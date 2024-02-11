using Application.FinishAuction;
using MassTransit;
using MediatR;
using Shared.Domain.Events;

namespace WebApi.Consumer;

public class AuctionFinishedConsumer(ISender sender) : IConsumer<AuctionFinished>
{
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        var command = new FinishAuctionCommand(
            context.Message.AuctionId,
            context.Message.ItemSold,
            context.Message.Winner,
            context.Message.Seller,
            context.Message.Amount);
        await sender.Send(command);
    }
}