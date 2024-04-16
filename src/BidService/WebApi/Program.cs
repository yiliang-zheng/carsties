using Infrastructure;
using Serilog;
using System.Reflection;
using Application;
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

            app.UseAuthorization();

            if (!app.Environment.IsProduction())
            {
                await app.Services.MigrateDatabase();
            }

            var grouped = app.MapGroup("/api/bids");
            app.MapEndpoints(grouped);
            
            await app.RunAsync();
        }
    }
}
