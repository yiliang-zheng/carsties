using Shared.Domain;

namespace Domain.Auction.Event;

public record AuctionUpdated(
    Guid Id,
    string Make,
    string Model,
    string Color,
    int Mileage,
    int Year
    ) : DomainEventBase;