using Domain.Auction;
using Domain.Auction.Specification;
using FluentResults;
using MediatR;
using Shared.Domain.Interface;

namespace Application.SearchFinishAuction;

public class SearchFinishAuctionCommandHandler(IRepository<Auction> auctionRepository) : IRequestHandler<SearchFinishAuctionCommand, Result<List<Guid>>>
{
    public async Task<Result<List<Guid>>> Handle(SearchFinishAuctionCommand request, CancellationToken cancellationToken)
    {
        var searchFinishAuctionSpec = new FinishAuctionSearchSpec();
        var result = (await auctionRepository.ListAsync(searchFinishAuctionSpec, cancellationToken))
            .Select(p => p.Id)
            .ToList();

        return result;
    }
}