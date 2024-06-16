using AutoMapper;
using Domain.Bid;
using Domain.Bid.Repository;
using Domain.Bid.Specification;
using FluentResults;
using FluentResults.Extensions;
using MediatR;

namespace Application.ListBids;

public class ListBidsCommandHandler(IBidRepository bidRepository, IMapper mapper) : IRequestHandler<ListBidsCommand, Result<List<BidDto>>>
{
    /// <summary>
    /// 1. create spec
    /// 2. get bids by spec
    /// 3. map to bid dto
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<BidDto>>> Handle(ListBidsCommand request, CancellationToken cancellationToken)
    {
        var result = await CreateSpec(request)
            .Bind(spec => GetBids(spec, cancellationToken))
            .Bind(MapDto);

        return result;
    }

    private Result<BidsByAuctionSpec> CreateSpec(ListBidsCommand request)
    {
        var spec = new BidsByAuctionSpec(request.AuctionId);
        return Result.Ok(spec);
    }

    private async Task<Result<List<Bid>>> GetBids(BidsByAuctionSpec spec, CancellationToken cancellationToken)
    {
        var entities = await bidRepository.ListAsync(spec, cancellationToken);
        if (entities is null) return Result.Fail<List<Bid>>("bids not found");

        return Result.Ok(entities);
    }

    private Result<List<BidDto>> MapDto(List<Bid> entities)
    {
        var result = entities.Select(mapper.Map<BidDto>).ToList();
        return result;
    }
}