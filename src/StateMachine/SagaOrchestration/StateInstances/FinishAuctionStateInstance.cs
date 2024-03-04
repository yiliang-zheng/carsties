using MassTransit;

namespace SagaOrchestration.StateInstances;

public class FinishAuctionStateInstance:SagaStateMachineInstance
{
    public Guid CorrelationId
    {
        get;
        set;
    }

    public string CurrentState { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public Guid AuctionId { get; set; }

    public uint RowVersion { get; set; }

}