using MediatR;

namespace Shared.Domain;

public abstract record DomainEventBase : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}