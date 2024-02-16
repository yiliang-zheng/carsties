using Application.UpdateBid;
using Domain.Auction;
using MassTransit;
using MediatR;
using Shared.Domain.Events;

namespace WebApi.Consumer;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly ISender _sender;
    private readonly ILogger<BidPlacedConsumer> _logger;

    public BidPlacedConsumer(ISender sender, ILogger<BidPlacedConsumer> logger)
    {
        _sender = sender;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        this._logger.LogInformation("--> Received BidPlaced event: {AuctionID} with Message ID {MessageID}",
            context.Message.AuctionId,
            context.MessageId
        );

        var command = new PlaceBidCommand(context.Message.AuctionId,
            BidStatus.FromName(context.Message.BidStatus),
            context.Message.Amount
        );

        var result = await this._sender.Send(command);
        this._logger.LogInformation(
            "-->Received result from PlaceBidCommandHandler: {Success}, Auction ID: {AuctionID}, CurrentHighestBid: {CurrentHighestBid}",
            result.IsSuccess,
            result.Value.Id,
            result.ValueOrDefault.CurrentHighBid
        );
    }
}