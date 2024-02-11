namespace WebApi.Auction;

public class GetAuctionRequest
{
    public const string Route = "/api/auctions/{Id}";

    public Guid Id { get; set; }
}