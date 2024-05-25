using Ardalis.GuardClauses;

namespace Domain.Bid;

public class Auction
{
    public Guid Id { get; private set; }

    public DateTimeOffset AuctionEnd { get; private set; }

    public string Seller { get; private set; }

    public int ReservePrice { get; private set; }

    public bool Finished { get; private set; }

    public bool ItemSold { get; private set; }

    public string Winner { get; private set; }

    public Auction(Guid id, DateTimeOffset auctionEnd, string seller, int reservePrice, bool finished, bool itemSold, string winner)
    {
        Id = Guard.Against.Default(id);
        AuctionEnd = auctionEnd;
        Seller = seller;
        ReservePrice = Guard.Against.NegativeOrZero(reservePrice);
        Finished = finished;
        ItemSold = itemSold;
        Winner = winner;
    }

    public void FinishAuction(Bid? winningBid)
    {
        switch (winningBid)
        {
            case null:
                ItemSold = false;
                Winner = string.Empty;
                break;
            case { } x:
                ItemSold = true;
                Winner = x.Bidder;
                break;
        }

        Finished = true;
    }

    public void RollbackFinishAuction()
    {
        Finished  = false;
        ItemSold = false;
        Winner = string.Empty;
    }
}