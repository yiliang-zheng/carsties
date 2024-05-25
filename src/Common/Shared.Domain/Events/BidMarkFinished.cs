using MassTransit;

namespace Shared.Domain.Events;

public class BidMarkFinished : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }

    public Guid AuctionId { get; init; }

    public string Winner { get; init; }

    public int? SoldAmount { get; init; }

    public bool ItemSold { get; init; }

    public DateTimeOffset CreatedDate { get; init; }
}

public class BidMarkFinishedFailed : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }

    public Guid AuctionId { get; init; }

    public DateTimeOffset CreatedDate { get; init; }

    public Exception FailedException { get; init; }
}