using Application;
using FastEndpoints;
using Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IO.Compression;
using System.Reflection;
using Npgsql;
using Npgsql.Util;
using Polly;
using WebApi.Consumer;
using WebApi.Grpc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddFastEndpoints();
builder.Services.AddAutoMapper(
    Assembly.GetAssembly(typeof(Program))
);
builder.Services.AddMassTransit(config =>
{
    config.AddEntityFrameworkOutbox<AppDbContext>(outboxConfigurator =>
    {
        outboxConfigurator.QueryDelay = TimeSpan.FromSeconds(10);
        outboxConfigurator.UsePostgres();
        outboxConfigurator.UseBusOutbox();
    });
    config.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
    config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        cfg.ReceiveEndpoint(
            "mark-auction-finish",
            configEndpoint => configEndpoint.ConfigureConsumer<FinishAuctionMessageConsumer>(context)
        );

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityService:Authority"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            NameClaimType = "username"
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.UseResponseCompression();
app.UseFastEndpoints();
app.MapGrpcService<GrpcAuctionService>();

//seed data
if (!app.Environment.IsProduction())
{
    await app.Services.InitializeDatabase();
}
app.Run();


