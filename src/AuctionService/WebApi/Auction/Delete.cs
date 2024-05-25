using Application.DeleteAuction;
using FastEndpoints;
using FluentResults;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace WebApi.Auction;

public class Delete : EndpointWithoutRequest
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public Delete(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    public string CurrentUser => User.Identity switch
    {
        { Name.Length: > 0 } => User.Identity.Name,
        { Name.Length: <= 0 } => string.Empty,
        null => string.Empty
    };

    public override void Configure()
    {
        Delete(DeleteAuctionRequest.Route);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var command = new DeleteAuctionCommand(Route<Guid>("Id"), CurrentUser);
        var result = await this._sender.Send(command, ct);
        if (result.IsFailed)
        {
            foreach (var resultError in result.Errors)
            {
                AddError(resultError.Message);
            }

            await this.SendErrorsAsync(400, ct);
            return;
        }

        await this.SendOkAsync(ct);
    }
}