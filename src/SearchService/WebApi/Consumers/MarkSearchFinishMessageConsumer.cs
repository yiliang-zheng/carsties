using AutoMapper;
using MassTransit;
using Shared.Domain.Events;
using Shared.Domain.Messages;
using WebApi.Repositories;

namespace WebApi.Consumers;

public class MarkSearchFinishMessageConsumer: IConsumer<MarkSearchFinishMessage>
{
    private readonly ILogger<AuctionCreatedConsumer> _logger;
    private readonly ISearchRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public MarkSearchFinishMessageConsumer(ILogger<AuctionCreatedConsumer> logger, ISearchRepository repository, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }
    public async Task Consume(ConsumeContext<MarkSearchFinishMessage> context)
    {
        try
        {
            this._logger.LogInformation(
                "--> Search service received message: {Event} with Auction ID: {AuctionID}",
                nameof(MarkAuctionFinishMessage),
                context.Message.AuctionId
            );

            await this._repository.MarkFinished(
                context.Message.AuctionId,
                context.Message.Status,
                context.Message.Winner,
                context.Message.SoldAmount
            );

            await this._publishEndpoint.Publish(new SearchMarkFinished
            {
                AuctionId = context.Message.AuctionId,
                CorrelationId = context.Message.CorrelationId,
                CreatedDate = DateTimeOffset.UtcNow
            });
        }
        catch (Exception e)
        {
            await this._publishEndpoint.Publish(new SearchMarkFinishedFailed
            {
                AuctionId = context.Message.AuctionId,
                CorrelationId = context.Message.CorrelationId,
                CreatedDate = DateTimeOffset.UtcNow,
                FailedException = e
            });
        }
        
    }
}