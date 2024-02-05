using System.Reflection;
using Application;
using FastEndpoints;
using Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Filters;
using WebApi.Consumer;

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


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.UseFastEndpoints();

//seed data
if (!app.Environment.IsProduction())
{
    await app.Services.InitializeDatabase();
}
app.Run();


