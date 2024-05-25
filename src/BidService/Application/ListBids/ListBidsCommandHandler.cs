using AutoMapper;
using Domain.Bid.Repository;
using Domain.Bid.Specification;
using FluentResults;
using MediatR;

namespace Application.ListBids;

public class ListBidsCommandHandler(IBidRepository bidRepository, IMapper mapper) : IRequestHandler<ListBidsCommand, Result<List<BidDto>>>
{
    
    public async Task<Result<List<BidDto>>> Handle(ListBidsCommand request, CancellationToken cancellationToken)
    {
        var spec = new BidsByAuctionSpec(request.AuctionId);
        var entities = await bidRepository.ListAsync(spec, cancellationToken);
        if (entities is null) return Result.Fail<List<BidDto>>("bids not found");

        var result = entities.Select(mapper.Map<BidDto>).ToList();
        return result;
    }
}