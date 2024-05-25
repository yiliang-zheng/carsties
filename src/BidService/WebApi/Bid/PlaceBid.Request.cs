namespace WebApi.Bid;

public sealed record PlaceBideRequest(string Bidder, int Amount, Guid AuctionId);