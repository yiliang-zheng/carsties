using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Domain;

public abstract class EntityBase
{
    private readonly List<DomainEventBase> _domainEvents = new();
    public Guid Id { get; protected set; }

    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => this._domainEvents.AsReadOnly();

    public void RegisterDomainEvent(DomainEventBase domainEvent) => this._domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => this._domainEvents.Clear();
}