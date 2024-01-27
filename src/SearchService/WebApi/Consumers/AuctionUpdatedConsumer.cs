using MassTransit;
using Shared.Domain.Events;
using WebApi.Repositories;

namespace WebApi.Consumers;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly ISearchRepository _searchRepository;
    private readonly ILogger<AuctionCreatedConsumer> _logger;

    public AuctionUpdatedConsumer(ISearchRepository searchRepository, ILogger<AuctionCreatedConsumer> logger)
    {
        _searchRepository = searchRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        this._logger.LogInformation($"-->Received AuctionUpdated event: {context.Message.Id}");
        var (id, make, model, color, mileage, year) = context.Message;
        await this._searchRepository.UpdateItem(
            id,
            make,
            model,
            color,
            mileage,
            year
        );
    }
}