using Shared.Domain.Events;

namespace Hub.Hubs;

public class NotificationHub: Microsoft.AspNetCore.SignalR.Hub<INotificationHubClient>
{
    public async Task SendAuctionCreatedNotification(AuctionCreated auction)
    {
        await Clients.All.ReceiveAuctionCreatedNotification(auction);
    }

    public async Task SendAuctionFinishedNotification(AuctionFinished auctionFinished)
    {
        await Clients.All.ReceiveAuctionFinishedNotification(auctionFinished);
    }

    public async Task SendBidPlacedNotification(BidPlaced bidPlaced)
    {
        await Clients.All.ReceiveBidPlacedNotification(bidPlaced);
    }
}