using MassTransit;
using Shared.Domain.Events;
using WebApi.Repositories;

namespace WebApi.Consumers;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly ISearchRepository _searchRepository;

    public AuctionUpdatedConsumer(ISearchRepository searchRepository)
    {
        _searchRepository = searchRepository;
    }

    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
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