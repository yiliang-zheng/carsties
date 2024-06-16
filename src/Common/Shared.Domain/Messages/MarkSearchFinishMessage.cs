using MassTransit;

namespace Shared.Domain.Messages;

public class MarkSearchFinishMessage : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }

    public Guid AuctionId { get; init; }

    public string Status { get; init; }

    public string Winner { get; init; }

    public int? SoldAmount { get; init; }

    public string Seller { get; init; }

    public bool ItemSold { get; init; }
}