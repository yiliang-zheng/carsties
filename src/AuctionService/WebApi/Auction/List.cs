using Application;
using Application.SearchAuction;
using FastEndpoints;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace WebApi.Auction;

public class List(ISender sender, ILogger<List> logger, IMapper mapper) : Endpoint<ListAuctionRequest, List<AuctionDto>>
{
    public override void Configure()
    {
        Get(ListAuctionRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ListAuctionRequest req, CancellationToken ct)
    {
        var command = mapper.Map<SearchAuctionCommand>(req);
        var result = await sender.Send(command, ct);
        if (result.IsFailed)
        {
            foreach (var error in result.Errors)
            {
                logger.LogError(error.Message);
                AddError(error.Message);
            }

            await this.SendErrorsAsync(400, ct);
            return;
        }

        await SendOkAsync(result.Value, ct);
    }
}