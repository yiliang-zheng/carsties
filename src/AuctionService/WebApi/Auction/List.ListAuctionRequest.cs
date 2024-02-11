namespace WebApi.Auction;

public class ListAuctionRequest
{
    public const string Route = "/api/auctions";

    public DateTime? From { get; set; }
}