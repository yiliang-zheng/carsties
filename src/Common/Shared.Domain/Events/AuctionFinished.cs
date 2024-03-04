using MassTransit;

namespace Shared.Domain.Events;

public class AuctionFinished : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }

    public Guid AuctionId { get; init; }

    public string Winner { get; init; }

    public int? SoldAmount { get; set; }

    public string Status { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
}

public class AuctionFinishFailed : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }

    public Guid AuctionId { get; init; }

    public Exception FailedException { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
}