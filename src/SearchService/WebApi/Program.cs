using FastEndpoints;
using MassTransit;
using Polly;
using Polly.Retry;
using System.Net;
using System.Reflection;
using WebApi.Consumers;
using WebApi.Data;
using WebApi.Repositories;
using WebApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//automapper
builder.Services.AddAutoMapper(
    Assembly.GetAssembly(typeof(Program))
);

builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddHttpClient<AuctionSvcHttpClient>()
    .AddResilienceHandler("http-pipeline", (pipelineBuilder) =>
    {
        pipelineBuilder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
        {
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .Handle<Exception>()
                .HandleResult(message => message.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.InternalServerError),
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(3),
            OnRetry = (args) =>
            {
                Console.WriteLine($"--> Retry attempt: {args.AttemptNumber}");
                return default;
            }
        });
    });
builder.Services.AddFastEndpoints();

//mass transit
builder.Services.AddMassTransit(config =>
{
    config.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    //format exchange name
    config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(
        "search",
        false)
    );

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseFastEndpoints();
app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DatabaseInitializer.InitDb(app);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        //ignored
    }
});


app.Run();


