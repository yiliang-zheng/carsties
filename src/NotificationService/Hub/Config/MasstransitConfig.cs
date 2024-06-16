using Hub.Consumers;
using MassTransit;
using MassTransit.Configuration;

namespace Hub.Config;

public static class MasstransitConfig
{
    public static IServiceCollection AddMasstransit(this IServiceCollection services, IConfiguration config)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
            busConfigurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("notification", false));
            busConfigurator.UsingRabbitMq((context, rabbitmqConfigurator) =>
            {
                rabbitmqConfigurator.Host(config["RabbitMq:Host"], "/", host =>
                {
                    host.Username(config.GetValue("RabbitMq:Username", "guest")!);
                    host.Password(config.GetValue("RabbitMq:Password", "guest")!);
                });

                rabbitmqConfigurator.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}