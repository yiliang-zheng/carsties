using Application;
using Application.SearchAuction;
using FastEndpoints;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace WebApi.Auction;

public class List : Endpoint<ListAuctionRequest, List<AuctionDto>>
{
    private readonly ISender _sender;
    private readonly ILogger<List> _logger;
    private readonly IMapper _mapper;

    public List(ISender sender, ILogger<List> logger, IMapper mapper)
    {
        _sender = sender;
        _logger = logger;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Get(ListAuctionRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ListAuctionRequest req, CancellationToken ct)
    {
        var command = this._mapper.Map<SearchAuctionCommand>(req);
        var result = await this._sender.Send(command, ct);
        if (result.IsFailed)
        {
            foreach (var error in result.Errors)
            {
                this._logger.LogError(error.Message);
                AddError(error.Message);
            }

            await this.SendErrorsAsync(400, ct);
            return;
        }

        await SendOkAsync(result.Value, ct);
    }
}