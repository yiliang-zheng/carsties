using Domain.Auction;
using Infrastructure.Data;
using Infrastructure.Data.Repository;
using Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interface;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
            services.AddScoped<IRepository<Auction>, AuctionRepository>();
            services.AddScoped<DatabaseInitializer>();
            services.AddDbContext<AppDbContext>((sp,opts) =>
            {
                opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                opts.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
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
