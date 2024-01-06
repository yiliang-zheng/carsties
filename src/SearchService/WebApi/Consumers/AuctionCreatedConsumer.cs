using AutoMapper;
using MassTransit;
using MongoDB.Entities;
using Shared.Domain.Events;
using WebApi.Models;

namespace WebApi.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;

    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine($"--> consuming auction created events: {context.Message.Id}");
        var item = this._mapper.Map<Item>(context.Message);

        await item.SaveAsync();
    }
}