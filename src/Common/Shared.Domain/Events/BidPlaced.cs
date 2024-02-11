namespace Shared.Domain.Events;

public class BidPlaced : DomainEventBase
{
    public Guid BidId { get; init; }

    public Guid AuctionId { get; init; }

    public string Bidder { get; init; }

    public DateTimeOffset BidTime { get; init; }

    public int Amount { get; init; }

    public string BidStatus { get; init; }
}