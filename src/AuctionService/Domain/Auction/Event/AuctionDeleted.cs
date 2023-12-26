using Shared.Domain;

namespace Domain.Auction.Event;

public record AuctionDeleted(Guid Id) : DomainEventBase;