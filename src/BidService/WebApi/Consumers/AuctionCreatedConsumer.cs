using Domain.Bid;
using Domain.Bid.Repository;
using MassTransit;
using Shared.Domain.Events;
using Shared.Domain.Interface;

namespace WebApi.Consumers;

public class AuctionCreatedConsumer(IBidRepository bidRepository, IUnitOfWork unitOfWork) : IConsumer<AuctionCreated>
{
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        var auction = new Auction(
            context.Message.Id,
            context.Message.AuctionEnd,
            context.Message.Seller,
            context.Message.ReservePrice,
            context.Message.Status.Equals("Finished"),
            context.Message.SoldAmount > 0,
            context.Message.Winner);
        await bidRepository.CreateAuction(auction);
        await unitOfWork.SaveChangesAsync();
    }
}