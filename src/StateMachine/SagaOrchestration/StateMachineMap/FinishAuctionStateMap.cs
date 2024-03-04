using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaOrchestration.StateInstances;

namespace SagaOrchestration.StateMachineMap;

public class FinishAuctionStateMap:SagaClassMap<FinishAuctionStateInstance>
{
    protected override void Configure(EntityTypeBuilder<FinishAuctionStateInstance> entity, ModelBuilder model)
    {
        entity.Property(p => p.CurrentState).HasMaxLength(256);
        entity.Property(p => p.AuctionId);
        entity.Property(p => p.CreatedDate);

        entity.Property(p => p.RowVersion)
            .HasColumnName("xmin")
            .HasColumnType("xid")
            .IsRowVersion();
    }
}