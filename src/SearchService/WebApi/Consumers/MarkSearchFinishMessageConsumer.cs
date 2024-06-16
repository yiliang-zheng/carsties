using MassTransit;
using Shared.Domain.Events;
using Shared.Domain.Messages;
using WebApi.Repositories;

namespace WebApi.Consumers;

public class MarkSearchFinishMessageConsumer(
    ILogger<MarkSearchFinishMessageConsumer> logger,
    ISearchRepository repository,
    IPublishEndpoint publishEndpoint)
    : IConsumer<MarkSearchFinishMessage>
{

    public async Task Consume(ConsumeContext<MarkSearchFinishMessage> context)
    {
        try
        {
            logger.LogInformation(
                "--> Search service received message: {Event} with Auction ID: {AuctionID}",
                nameof(MarkAuctionFinishMessage),
                context.Message.AuctionId
            );

            await repository.MarkFinished(
                context.Message.AuctionId,
                context.Message.Status,
                context.Message.Winner,
                context.Message.SoldAmount
            );

            await publishEndpoint.Publish(new SearchMarkFinished
            {
                AuctionId = context.Message.AuctionId,
                CorrelationId = context.Message.CorrelationId,
                SoldAmount = context.Message.SoldAmount,
                ItemSold = context.Message.ItemSold,
                Seller = context.Message.Seller,
                Winner = context.Message.Winner,
                CreatedDate = DateTimeOffset.UtcNow
            });

            logger.LogInformation("--> Search Service: Published {Event} with CorrelationId: {CorrelationId}", nameof(SearchMarkFinished), context.Message.CorrelationId);
        }
        catch (Exception e)
        {
            await publishEndpoint.Publish(new SearchMarkFinishedFailed
            {
                AuctionId = context.Message.AuctionId,
                CorrelationId = context.Message.CorrelationId,
                CreatedDate = DateTimeOffset.UtcNow,
                FailedException = e
            });
        }
        
    }
}