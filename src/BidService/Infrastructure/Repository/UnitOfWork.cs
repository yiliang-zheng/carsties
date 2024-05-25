using Domain.Bid;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Constants.Extensions;
using Shared.Domain;
using Shared.Domain.Events;
using Shared.Domain.Interface;

namespace Infrastructure.Repository;

public class UnitOfWork(AppDbContext dbContext, IPublishEndpoint publishEndpoint) : IUnitOfWork
{

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await UpdateAuditProperties();
        await DispatchDomainEvents();
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEvents()
    {
        if (dbContext == null) return;

        var entities = dbContext.ChangeTracker
            .Entries<EntityBase>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(p => p.DomainEvents).ToList();

        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            switch (domainEvent)
            {
                case BidPlaced bidPlaced:
                    await publishEndpoint.Publish(bidPlaced);
                    break;
                default:
                    break;
            }

        }
    }

    private Task UpdateAuditProperties()
    {
        if (dbContext == null) return Task.CompletedTask;
        foreach (var entry in dbContext.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = null;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return Task.CompletedTask;
    }
}