using MassTransit;

namespace Shared.Domain.Events;

public class AuctionMarkFinished : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }

    public Guid AuctionId { get; init; }

    public string Seller { get; init; }

    public string Winner { get; init; }

    public int? SoldAmount { get; init; }

    public string Status { get; init; }

    public bool ItemSold { get; init; }

    public DateTimeOffset CreatedDate { get; init; }
}

public class AuctionMarkFinishFailed : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }

    public Guid AuctionId { get; init; }

    public Exception FailedException { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
}