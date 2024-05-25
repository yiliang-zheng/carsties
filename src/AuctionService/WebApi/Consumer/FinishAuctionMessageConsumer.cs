using Application.FinishAuction;
using MassTransit;
using MediatR;
using Shared.Domain.Events;
using Shared.Domain.Messages;

namespace WebApi.Consumer;

public class FinishAuctionMessageConsumer(ISender sender, IPublishEndpoint publishEndpoint, ILogger<FinishAuctionMessageConsumer> logger)
    : IConsumer<FinishAuctionMessage>
{
    public async Task Consume(ConsumeContext<FinishAuctionMessage> context)
    {
        try
        {
            var command = new FinishAuctionCommand(
                context.Message.AuctionId,
                context.Message.ItemSold,
                context.Message.Winner,
                context.Message.Seller,
                context.Message.Amount);
            var result = await sender.Send(command);
            if (result.IsFailed) throw new Exception("failed to mark auction as finished");

            await publishEndpoint.Publish(new AuctionFinished
            {
                AuctionId = context.Message.AuctionId,
                CorrelationId = context.Message.CorrelationId,
                CreatedDate = DateTimeOffset.UtcNow,
                SoldAmount = result.Value.SoldAmount,
                Status = result.Value.Status,
                Winner = result.Value.Winner
            });

            logger.LogInformation("--> Auction Service: Published {Event} with CorrelationId: {CorrelationId}", nameof(AuctionFinished), context.Message.CorrelationId);
        }
        catch (Exception e)
        {
            await publishEndpoint.Publish(new AuctionFinishFailed
            {
                AuctionId = context.Message.AuctionId,
                CorrelationId = context.Message.CorrelationId,
                CreatedDate = DateTimeOffset.UtcNow,
                FailedException = e
            });
        }

    }
}