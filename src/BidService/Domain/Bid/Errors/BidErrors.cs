using FluentResults;

namespace Domain.Bid.Errors;

public class BidErrors
{
    public static readonly Error AuctionNotFound = new ("Auction not found.");

    public static readonly Error SameBidderAndSeller = new Error("Bidder and seller cannot be the same.");
}