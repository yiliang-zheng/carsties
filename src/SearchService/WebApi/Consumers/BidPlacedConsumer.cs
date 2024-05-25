using MassTransit;
using Shared.Domain.Events;
using WebApi.Repositories;

namespace WebApi.Consumers;

public class BidPlacedConsumer(ISearchRepository repository, ILogger<BidPlacedConsumer> logger) : IConsumer<BidPlaced>
{
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        logger.LogInformation("Received BidPlaced event: {AuctionID}", context.Message.AuctionId);

        await repository.UpdateCurrentHighestBid(context.Message.AuctionId, context.Message.Amount);
    }
}