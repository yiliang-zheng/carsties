using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaOrchestration;
using SagaOrchestration.DbContext;
using SagaOrchestration.StateInstances;
using SagaOrchestration.StateMachines;
using Serilog;
using Shared.Domain.Messages;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddMassTransit(config =>
{
    config.AddRequestClient<CheckAuctionFinish>();
    config.AddSagaStateMachine<FinishAuctionStateMachine, FinishAuctionStateInstance>()
        .EntityFrameworkRepository(repoConfigurator =>
        {
            repoConfigurator.ConcurrencyMode = ConcurrencyMode.Optimistic;
            repoConfigurator.AddDbContext<DbContext, StateMachineDbContext>((provider, dbContextBuilder) =>
            {
                dbContextBuilder.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    m =>
                    {
                        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        m.MigrationsHistoryTable($"__{nameof(StateMachineDbContext)}");
                    });
            });
            repoConfigurator.UsePostgres();
        });
    config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("saga", false));

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        cfg.ReceiveEndpoint("saga-mark-auction-finish-message", configEndpoint =>
        {
            configEndpoint.ConfigureSaga<FinishAuctionStateInstance>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddDbContext<StateMachineDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHostedService<Worker>();

var host = builder
    .Build();

if (builder.Environment.IsDevelopment())
{
    try
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StateMachineDbContext>();
        await dbContext.Database.MigrateAsync();
    }
    catch (Exception e)
    {
        Log.Logger.Error("Error occurred during database migration: {Exception}", e);
    }

}

host.Run();
