﻿using Domain.Auction;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Shared.Constants.Extensions;
using Shared.Domain;
using Shared.Domain.Events;

namespace Infrastructure.Interceptors;

public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IServiceProvider _serviceProvider;

    public DispatchDomainEventsInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var saveResult = base.SavedChanges(eventData, result);
        this.DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return saveResult;
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var saveResult = await base.SavedChangesAsync(eventData, result, cancellationToken);
        await this.DispatchDomainEvents(eventData.Context);
        return saveResult;
    }

    public async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null) return;
        using var scope = _serviceProvider.CreateScope();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var entities = context.ChangeTracker
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
                        await publishEndpoint.Publish(auctionCreated);
                        break;
                    case AuctionDeleted auctionDeleted:
                        await publishEndpoint.Publish(auctionDeleted);
                        break;
                    case AuctionUpdated auctionUpdated:
                        await publishEndpoint.Publish(auctionUpdated);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}