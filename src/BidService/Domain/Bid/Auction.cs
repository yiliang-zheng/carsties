using Ardalis.GuardClauses;
using Shared.Domain;

namespace Domain.Bid;

public class Auction
{
    public Guid Id { get; private set; }

    public DateTimeOffset AuctionEnd { get; private set; }

    public string Seller { get; private set; }

    public int ReservePrice { get; private set; }

    public bool Finished { get; private set; }

    public Auction(Guid id, DateTimeOffset auctionEnd, string seller, int reservePrice, bool finished)
    {
        Id = Guard.Against.Default(id);
        AuctionEnd = auctionEnd;
        Seller = seller;
        ReservePrice = Guard.Against.NegativeOrZero(reservePrice);
        Finished = finished;
    }
}