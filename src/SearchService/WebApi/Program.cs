using FastEndpoints;
using MassTransit;
using Polly;
using Polly.Retry;
using System.Net;
using System.Reflection;
using Serilog;
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

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

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
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        cfg.ReceiveEndpoint("search-auction-created", configEndpoint =>
        {
            configEndpoint.UseMessageRetry(retryConfig =>
            {
                retryConfig.Interval(3, TimeSpan.FromSeconds(3));
            });

            configEndpoint.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("search-auction-updated", configEndpoint =>
        {
            configEndpoint.UseMessageRetry(retryConfig =>
            {
                retryConfig.Interval(3, TimeSpan.FromSeconds(3));
            });
            configEndpoint.ConfigureConsumer<AuctionUpdatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("search-auction-deleted", configEndpoint =>
        {
            configEndpoint.UseMessageRetry(retryConfig =>
            {
                retryConfig.Interval(3, TimeSpan.FromSeconds(3));
            });
            configEndpoint.ConfigureConsumer<AuctionDeletedConsumer>(context);
        });

        cfg.ReceiveEndpoint("mark-search-auction-finish", configEndpoint =>
        {
            configEndpoint.ConfigureConsumer<MarkSearchFinishMessageConsumer>(context);
        });

        //global retry policy 
        //cfg.UseMessageRetry(retryConfig =>
        //{
        //    retryConfig.Interval(5, 5);

        //});
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


