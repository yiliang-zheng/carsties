﻿using Application;
using Application.SearchAuction;
using FastEndpoints;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace WebApi.Auction;

public class List : Endpoint<ListAuctionRequest, List<AuctionDto>>
{
    private readonly ILogger<WebApi.Auction.List> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public List(ILogger<List> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Get(ListAuctionRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ListAuctionRequest req, CancellationToken ct)
    {
        _logger.LogInformation($"Received List request at {DateTimeOffset.Now}");
        var command = _mapper.Map<SearchAuctionCommand>(req);
        var result = await _sender.Send(command, ct);
        if (result.IsFailed)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError(error.Message);
                AddError(error.Message);
            }

            await this.SendErrorsAsync(400, ct);
            return;
        }

        await SendOkAsync(result.Value, ct);
    }
}