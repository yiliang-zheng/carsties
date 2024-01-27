using Domain.Auction;
using Infrastructure.Data;
using Infrastructure.Data.Repository;
using Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interface;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<AuditableEntityInterceptor>();

            services.AddScoped<IRepository<Auction>, AuctionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DatabaseInitializer>();
            services.AddDbContext<AppDbContext>((sp, opts) =>
            {
                opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }

        public static async Task InitializeDatabase(this IServiceProvider sp)
        {
            using var scope = sp.CreateScope();

            var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
            if (initializer is not null)
            {
                await initializer.Initialize();
            }
        }
    }
}
