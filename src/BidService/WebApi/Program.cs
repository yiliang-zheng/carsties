using Infrastructure;
using Serilog;
using System.Reflection;
using Application;
using Infrastructure.Grpc;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Polly;
using WebApi.Consumers;
using WebApi.Extensions;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // Add services to the container.
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            //mass transit
            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
                config.AddEntityFrameworkOutbox<AppDbContext>(outboxConfigurator =>
                {
                    outboxConfigurator.QueryDelay = TimeSpan.FromSeconds(10);
                    outboxConfigurator.UsePostgres();
                    outboxConfigurator.UseBusOutbox();
                });

                config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids", false));
                config.UsingRabbitMq((context, busConfigurator) =>
                {
                    busConfigurator.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
                    {
                        host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest")!);
                        host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest")!);
                    });

                    busConfigurator.ConfigureEndpoints(context);
                });
            });

            //jwt auth
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(config =>
                {
                    config.Authority = builder.Configuration["IdentityService:Authority"];
                    config.RequireHttpsMetadata = false;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        NameClaimType = "username",
                    };
                });

            //application & infrastructure
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddAuthorization();

            builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure the HTTP request pipeline.
            app.UseAuthentication();
            app.UseAuthorization();

            var grouped = app.MapGroup("/api/bids").WithTags("Bids");
            app.MapEndpoints(grouped);

            if (!app.Environment.IsProduction())
            {
                var retryPolicy = Policy.Handle<NpgsqlException>().WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(5));
                await retryPolicy.ExecuteAsync(async () =>
                {
                    await app.Services.MigrateDatabase();
                });
            }
            
            await app.RunAsync();
        }
    }
}
