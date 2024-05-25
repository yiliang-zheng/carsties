using Application.SearchFinishAuction;
using MassTransit;
using MediatR;
using Shared.Domain.Messages;
using Shared.Domain.Response;

namespace WebApi.Consumer;

public class CheckAuctionFinishConsumer(ILogger<CheckAuctionFinishConsumer> logger, ISender sender) : IConsumer<CheckAuctionFinish>
{
    public async Task Consume(ConsumeContext<CheckAuctionFinish> context)
    {
        var command = new SearchFinishAuctionCommand();
        var result = await sender.Send(command);
        if (result.IsFailed)
        {
            result.Errors.ForEach(@error =>
            {
                logger.LogError("Error occurred during search finish auction: {Error}", @error.Message);
            });

            await context.RespondAsync(new CheckAuctionFinishResponse([]));
            return;
        }

        await context.RespondAsync(new CheckAuctionFinishResponse(result.Value));
    }
}