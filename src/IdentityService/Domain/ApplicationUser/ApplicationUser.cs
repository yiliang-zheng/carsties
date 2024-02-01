using Microsoft.AspNetCore.Identity;
using Shared.Domain;
using Shared.Domain.Interface;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.ApplicationUser;

public class ApplicationUser : IdentityUser, IAggregateRoot
{
    private readonly List<DomainEventBase> _domainEvents = new();

    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => this._domainEvents.AsReadOnly();

    public void RegisterDomainEvent(DomainEventBase domainEvent) => this._domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => this._domainEvents.Clear();
}