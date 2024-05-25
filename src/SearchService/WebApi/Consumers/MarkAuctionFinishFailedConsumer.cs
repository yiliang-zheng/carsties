using MassTransit;
using Shared.Domain.Events;
using WebApi.Repositories;

namespace WebApi.Consumers;

public class MarkAuctionFinishFailedConsumer(ISearchRepository searchRepository, ILogger<MarkAuctionFinishFailedConsumer> logger): IConsumer<MarkAuctionFinishFailed>
{
    public async Task Consume(ConsumeContext<MarkAuctionFinishFailed> context)
    {
        logger.LogInformation("Received {EventName} event. AuctionID: {AuctionID}", nameof(MarkAuctionFinishFailed), context.Message.AuctionId);
        await searchRepository.RollbackFinishedAuction(context.Message.AuctionId);
    }
}