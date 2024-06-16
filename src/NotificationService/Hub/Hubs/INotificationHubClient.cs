using Shared.Domain.Events;

namespace Hub.Hubs;

public interface INotificationHubClient
{
    Task ReceiveAuctionCreatedNotification(AuctionCreated auctionCreated);

    Task ReceiveAuctionFinishedNotification(AuctionFinished auctionFinished);

    Task ReceiveBidPlacedNotification(BidPlaced bidPlaced);
}