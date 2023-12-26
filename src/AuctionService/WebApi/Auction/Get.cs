using Application;
using Application.GetAuction;
using FastEndpoints;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace WebApi.Auction;

public class Get:Endpoint<GetAuctionRequest, AuctionDto>
{
    private readonly ISender _sender;
    private readonly ILogger<Get> _logger;
    private readonly IMapper _mapper;

    public Get(ISender sender, ILogger<Get> logger, IMapper mapper)
    {
        _sender = sender;
        _logger = logger;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get(GetAuctionRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAuctionRequest req, CancellationToken ct)
    {
        var command = this._mapper.Map<GetAuctionCommand>(req);
        var result = await this._sender.Send(command, ct);
        if (result.IsFailed)
        {
            foreach (var resultError in result.Errors)
            {
                this._logger.LogError(resultError.Message);
                AddError(resultError.Message);
            }

            await this.SendErrorsAsync(400, ct);
            return;
        }

        await this.SendOkAsync(result.Value, ct);
    }
}