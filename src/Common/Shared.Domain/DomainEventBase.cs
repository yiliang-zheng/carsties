using MediatR;

namespace Shared.Domain;

public abstract class DomainEventBase 
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}

