using Application;
using Application.MarkAuctionFinish;
using MassTransit;
using MediatR;
using Shared.Domain.Events;
using Shared.Domain.Messages;

namespace WebApi.Consumers;

public class MarkBidFinishMessageConsumer(ISender sender, IPublishEndpoint publishEndpoint, ILogger<MarkBidFinishMessageConsumer> logger) : IConsumer<MarkBidFinishMessage>
{
    public async Task Consume(ConsumeContext<MarkBidFinishMessage> context)
    {
        var command = new MarkAuctionFinishCommand(context.Message.AuctionId);
        var result = await sender.Send(command);

        var auctionDto = new AuctionDto();
        if (result.IsSuccess) auctionDto = result.Value;

        await publishEndpoint.Publish(new BidMarkFinished
        {
            CorrelationId = context.Message.CorrelationId,
            AuctionId = context.Message.AuctionId,
            Winner = auctionDto.Winner,
            ItemSold = auctionDto.ItemSold,
            SoldAmount = auctionDto.SoldAmount,
            CreatedDate = DateTimeOffset.UtcNow
        });

        logger.LogInformation("--> Bid Service: Published {Event} with CorrelationId: {CorrelationId}", nameof(BidMarkFinished), context.Message.CorrelationId);
    }
}