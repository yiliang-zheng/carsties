using Application.FinishAuction;
using MassTransit;
using MediatR;
using Shared.Domain.Events;
using Shared.Domain.Messages;

namespace WebApi.Consumer;

public class FinishAuctionMessageConsumer : IConsumer<FinishAuctionMessage>
{
    private readonly ISender _sender;
    private readonly IPublishEndpoint _publishEndpoint;

    public FinishAuctionMessageConsumer(ISender sender, IPublishEndpoint publishEndpoint)
    {
        _sender = sender;
        _publishEndpoint = publishEndpoint;
    }

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
            var result = await _sender.Send(command);
            if (result.IsFailed) throw new Exception("failed to mark auction as finished");

            await _publishEndpoint.Publish(new AuctionFinished
            {
                AuctionId = context.Message.AuctionId,
                CorrelationId = context.Message.CorrelationId,
                CreatedDate = DateTimeOffset.UtcNow,
                SoldAmount = result.Value.SoldAmount,
                Status = result.Value.Status,
                Winner = result.Value.Winner
            });
        }
        catch (Exception e)
        {
            await _publishEndpoint.Publish(new AuctionFinishFailed
            {
                AuctionId = context.Message.AuctionId,
                CorrelationId = context.Message.CorrelationId,
                CreatedDate = DateTimeOffset.UtcNow,
                FailedException = e
            });
        }
        
    }
}