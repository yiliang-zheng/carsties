using FluentResults;

namespace Domain.Auction.Errors;

public class AuctionErrors
{
    public static readonly Error AuctionNotFound = new Error("Auction not found.");
}