using FastEndpoints;
using Polly;
using Polly.Retry;
using System.Net;
using WebApi.Data;
using WebApi.Repositories;
using WebApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseFastEndpoints();
app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DatabaseInitializer.InitDb(app);
    }
    catch (Exception)
    {
        //ignored
    }
});


app.Run();


