using Ardalis.GuardClauses;
using Shared.Domain;
using Shared.Domain.Interface;

namespace Domain.Bid;

public class Bid : EntityBase, IAggregateRoot, IAuditableEntity
{
    public string Bidder { get; private set; }
    public DateTimeOffset BidTime { get; private set; }

    public int Amount { get; private set; }

    public BidStatus BidStatus { get; private set; }

    public Guid AuctionId { get; private set; }

    public Auction Auction { get; private set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Bid(Guid id, string bidder, DateTimeOffset bidTime, int amount, Guid auctionId)
    {
        Id = Guard.Against.Default(id);
        Bidder = Guard.Against.NullOrEmpty(bidder);
        BidTime = bidTime;
        Amount = Guard.Against.NegativeOrZero(amount);
        AuctionId = Guard.Against.Default(auctionId);
    }

    public void SetAuction(Auction auction)
    {
        Auction = Guard.Against.Null(auction);
    }

    public void SetBidStatus(List<Bid> otherBids)
    {
        //Auction already finished
        if (Auction.AuctionEnd < DateTimeOffset.UtcNow)
        {
            BidStatus = BidStatus.Finished;
            return;
        }

        if (otherBids is null or { Count: 0 })
        {
            BidStatus = Amount >= Auction.ReservePrice ? BidStatus.Accepted : BidStatus.AcceptedBelowReserve;
            return;
        }

        var currentHighestBid = otherBids.Select(p => p.Amount).Max();
        //current amount lower than current highest bid, return tooLow
        if (currentHighestBid >= Amount) BidStatus = BidStatus.TooLow;
        //current amount higher than current highest bid and higher than reserve price, return Accepted
        else if (Amount >= Auction.ReservePrice) BidStatus = BidStatus.Accepted;
        //current amount higher than current highest bid, but lower than reserve price, return AcceptedBelowReserve
        else BidStatus = BidStatus.AcceptedBelowReserve;
    }
}