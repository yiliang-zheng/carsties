using MassTransit;

namespace Shared.Domain.Messages;

public class FinishAuctionMessage : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }

    public Guid AuctionId { get; init; }

    public bool ItemSold { get; init; }

    public string Winner { get; init; }

    public string Seller { get; init; }

    public int? Amount { get; init; }

}