using Hub.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Shared.Domain.Events;

namespace Hub.Consumers;

public class AuctionFinishedConsumer(IHubContext<NotificationHub, INotificationHubClient> hubContext, ILogger<AuctionFinishedConsumer> logger):IConsumer<AuctionFinished>
{
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        logger.LogInformation("--> auction finished message received from {Consumer}", typeof(AuctionFinishedConsumer).FullName);
        await hubContext.Clients.All.ReceiveAuctionFinishedNotification(context.Message);
    }
}