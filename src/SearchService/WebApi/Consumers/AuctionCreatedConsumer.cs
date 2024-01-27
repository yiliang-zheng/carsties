using AutoMapper;
using MassTransit;
using MongoDB.Entities;
using Shared.Domain.Events;
using WebApi.Models;

namespace WebApi.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;
    private readonly ILogger<AuctionCreatedConsumer> _logger;

    public AuctionCreatedConsumer(IMapper mapper, ILogger<AuctionCreatedConsumer> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        this._logger.LogInformation($"--> Received message on : {context.Message.Id}");

        var item = this._mapper.Map<Item>(context.Message);

        await item.SaveAsync();
    }
}