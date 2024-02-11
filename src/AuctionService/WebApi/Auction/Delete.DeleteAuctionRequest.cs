namespace WebApi.Auction;

public class DeleteAuctionRequest
{
    public const string Route = "/api/auctions/{Id}";

    public Guid Id { get; set; }

    public string Seller { get; set; }
};