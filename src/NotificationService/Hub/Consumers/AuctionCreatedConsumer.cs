using Hub.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Shared.Domain.Events;

namespace Hub.Consumers;

public class AuctionCreatedConsumer(IHubContext<NotificationHub, INotificationHubClient> hubContext, ILogger<AuctionCreatedConsumer> logger): IConsumer<AuctionCreated>
{
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        logger.LogInformation("--> auction created message received from {Consumer}", typeof(AuctionCreatedConsumer).FullName);
        await hubContext.Clients.All.ReceiveAuctionCreatedNotification(context.Message);
    }
}