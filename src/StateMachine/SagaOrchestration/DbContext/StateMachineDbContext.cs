using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaOrchestration.StateMachineMap;

namespace SagaOrchestration.DbContext;

public class StateMachineDbContext:SagaDbContext
{
    public StateMachineDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new FinishAuctionStateMap();
        }
    }
}