namespace WebApi.Auction;

public class ListAuctionRequest
{
    public const string Route = "/api/auction";

    public DateTime? From { get; set; }
}