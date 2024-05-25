namespace Domain.Bid.Repository;

public interface IGrpcAuctionClient
{
    public Auction? GetAuction(Guid id);
}