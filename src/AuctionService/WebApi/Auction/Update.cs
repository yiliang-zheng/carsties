using Application;
using Application.UpdateAuction;
using FastEndpoints;
using MediatR;
using AutoMapper;

namespace WebApi.Auction;

public class Update: Endpoint<UpdateAuctionRequest, AuctionDto>
{
    private readonly AutoMapper.IMapper _mapper;
    private readonly ISender _sender;

    public Update(AutoMapper.IMapper mapper, ISender sender)
    {
        _mapper = mapper;
        _sender = sender;
    }

    public string CurrentUser => User.Identity switch
    {
        { Name.Length: > 0 } => User.Identity.Name,
        { Name.Length: <= 0 } => string.Empty,
        null => string.Empty
    };

    public override void Configure()
    {
        Put(UpdateAuctionRequest.Route);
    }

    public override async Task HandleAsync(UpdateAuctionRequest req, CancellationToken ct)
    {
        req.Seller = CurrentUser;
        var command = this._mapper.Map<UpdateAuctionCommand>(req);
        var result = await this._sender.Send(command, ct);
        if (result.IsFailed)
        {
            foreach (var resultError in result.Errors)
            {
                AddError(resultError.Message);
            }

            await SendErrorsAsync(400, ct);
            return;
        }

        await this.SendOkAsync(result.Value, ct);
    }
}