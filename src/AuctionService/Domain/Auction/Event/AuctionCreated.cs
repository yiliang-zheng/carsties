using MediatR;
using Shared.Domain;

namespace Domain.Auction.Event;

public record AuctionCreated(
    Guid Id,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime AuctionEnd,
    string Seller,
    string Winner,
    string Make,
    string Model,
    int Year,
    string Color,
    int Mileage,
    string ImageUrl,
    string Status,
    int ReservePrice,
    int? SoldAmount,
    int? CurrentHighBid
    ) : DomainEventBase;