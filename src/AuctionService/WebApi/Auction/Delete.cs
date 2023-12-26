﻿using Application.DeleteAuction;
using FastEndpoints;
using FluentResults;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace WebApi.Auction;

public class Delete:Endpoint<DeleteAuctionRequest, Result>
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public Delete(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Delete(DeleteAuctionRequest.Route);
    }

    public override async Task HandleAsync(DeleteAuctionRequest req, CancellationToken ct)
    {
        var command = this._mapper.Map<DeleteAuctionCommand>(req);
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