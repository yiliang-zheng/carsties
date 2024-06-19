using Domain.ApplicationUser;
using Domain.Grant;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interface;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DatabaseInitializer>();
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IRepository<Grant>, GrantRepository>();

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static async Task InitializeDatabase(this IServiceProvider sp)
    {
        using var scope = sp.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        if (initializer is not null)
        {
            await initializer.Seed();
        }

    }
}