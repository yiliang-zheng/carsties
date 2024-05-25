using Application.RollbackFinishAuction;
using MassTransit;
using MediatR;
using Shared.Domain.Events;

namespace WebApi.Consumer;

public class MarkAuctionFinishFailedConsumer(ISender sender, ILogger<MarkAuctionFinishFailedConsumer> logger) : IConsumer<MarkAuctionFinishFailed>
{
    public async Task Consume(ConsumeContext<MarkAuctionFinishFailed> context)
    {
        logger.LogInformation("Received {EventName} event. AuctionID: {AuctionID}", nameof(MarkAuctionFinishFailed), context.Message.AuctionId);
        var rollbackCommand = new RollbackFinishAuctionCommand(context.Message.AuctionId);
        await sender.Send(rollbackCommand);
    }
}