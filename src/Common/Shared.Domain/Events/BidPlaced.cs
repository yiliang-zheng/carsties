namespace Shared.Domain.Events;

public class BidPlaced : DomainEventBase
{
    public Guid BidId { get; init; }

    public Guid AuctionId { get; init; }

    public string Bidder { get; init; }

    public DateTimeOffset BidDateTime { get; init; }

    public int Amount { get; init; }

    public string BidStatus { get; init; }

    public DateTimeOffset AuctionEnd { get; init; }

    public string Seller { get; init; }

    public int ReservePrice { get; init; }

    public bool Finished { get; init; }
}