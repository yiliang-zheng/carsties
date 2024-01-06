using MassTransit;
using Shared.Domain.Events;
using WebApi.Repositories;

namespace WebApi.Consumers;

public class AuctionDeletedConsumer:IConsumer<AuctionDeleted>
{
    private readonly ISearchRepository _searchRepository;

    public AuctionDeletedConsumer(ISearchRepository searchRepository)
    {
        _searchRepository = searchRepository;
    }
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        Console.WriteLine($"--> received AuctionDeleted event: {context.Message.Id}");
        await this._searchRepository.DeleteItem(context.Message.Id);
    }
}