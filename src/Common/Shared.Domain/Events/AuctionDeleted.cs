namespace Shared.Domain.Events;

public class AuctionDeleted : DomainEventBase
{
    public Guid Id { get; set; }
}