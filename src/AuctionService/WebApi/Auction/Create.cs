﻿using Application;
using Application.CreateAuction;
using FastEndpoints;
using MediatR;

namespace WebApi.Auction;

public class Create : Endpoint<CreateAuctionRequest, AuctionDto>
{
    private readonly ISender _sender;
    private readonly AutoMapper.IMapper _mapper;

    public Create(ISender sender, AutoMapper.IMapper mapper)
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
        Post(CreateAuctionRequest.Route);
    }

    public override async Task HandleAsync(CreateAuctionRequest req, CancellationToken ct)
    {
        req.Seller = CurrentUser;
        var command = this._mapper.Map<CreateAuctionCommand>(req);
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