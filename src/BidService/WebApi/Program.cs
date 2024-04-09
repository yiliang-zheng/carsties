using Infrastructure;
using Serilog;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddAuthorization();


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            if (!app.Environment.IsProduction())
            {
                await app.Services.MigrateDatabase();
            }

            await app.RunAsync();
        }
    }
}
