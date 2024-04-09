using Ardalis.GuardClauses;
using Shared.Domain;
using Shared.Domain.Interface;

namespace Domain.Bid;

public class Bid: EntityBase, IAggregateRoot, IAuditableEntity
{
    public DateTimeOffset BidDateTime { get; private set; }

    public int Amount { get; private set; }

    public BidStatus BidStatus { get; private set; }

    public Guid AuctionId { get; private set; }

    public Auction Auction { get; private set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Bid(Guid id, DateTimeOffset bidDateTime, int amount, BidStatus bidStatus, Guid auctionId)
    {
        Id = Guard.Against.Default(id);
        BidDateTime = bidDateTime;
        Amount = Guard.Against.NegativeOrZero(amount);
        BidStatus = bidStatus;
        AuctionId = Guard.Against.Default(auctionId);
    }
}