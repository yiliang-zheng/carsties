namespace WebApi.Auction;

public class GetAuctionRequest
{
    public const string Route = "/api/auction/{Id}";

    public Guid Id { get; set; }
}