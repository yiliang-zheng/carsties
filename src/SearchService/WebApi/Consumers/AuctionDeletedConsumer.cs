using MassTransit;
using Shared.Domain.Events;
using WebApi.Repositories;

namespace WebApi.Consumers;

public class AuctionDeletedConsumer:IConsumer<AuctionDeleted>
{
    private readonly ISearchRepository _searchRepository;
    private readonly ILogger<AuctionDeletedConsumer> _logger;

    public AuctionDeletedConsumer(ISearchRepository searchRepository, ILogger<AuctionDeletedConsumer> logger)
    {
        _searchRepository = searchRepository;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        this._logger.LogInformation($"--> received AuctionDeleted event: {context.Message.Id}");
        await this._searchRepository.DeleteItem(context.Message.Id);
    }
}