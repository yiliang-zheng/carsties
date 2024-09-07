using Hub.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Shared.Domain.Events;

namespace Hub.Consumers;

public class BidPlacedConsumer(IHubContext<NotificationHub, INotificationHubClient> hubContext, ILogger<BidPlacedConsumer> logger) : IConsumer<BidPlaced>
{
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        logger.LogInformation("--> bid placed message received from {Consumer}", typeof(AuctionCreatedConsumer).FullName);
        await hubContext.Clients.All.ReceiveBidPlacedNotification(context.Message);
    }
}