using Domain.Auction;
using MassTransit;
using Shared.Constants.Extensions;
using Shared.Domain.Events;
using Shared.Domain;
using Shared.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await this.UpdateAuditProperties();
        await this.DispatchDomainEvents();
        await this._dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEvents()
    {
        if (_dbContext == null) return;

        var entities = _dbContext.ChangeTracker
            .Entries<EntityBase>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEventsDictionary = entities
            .ToDictionary(key => key, value => value.DomainEvents.ToList());

        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var pair in domainEventsDictionary)
        {
            foreach (var domainEvent in pair.Value)
            {
                switch (domainEvent)
                {
                    case AuctionCreated auctionCreated:
                        var createdAt = pair.Key.GetPropertyValue<DateTime>(nameof(Auction.CreatedAt));
                        auctionCreated.CreatedAt = createdAt;
                        await _publishEndpoint.Publish(auctionCreated);
                        break;
                    case AuctionDeleted auctionDeleted:
                        await _publishEndpoint.Publish(auctionDeleted);
                        break;
                    case AuctionUpdated auctionUpdated:
                        await _publishEndpoint.Publish(auctionUpdated);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private Task UpdateAuditProperties()
    {
        if (_dbContext == null) return Task.CompletedTask;
        foreach (var entry in _dbContext.ChangeTracker.Entries<IAuditableEntity>())
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