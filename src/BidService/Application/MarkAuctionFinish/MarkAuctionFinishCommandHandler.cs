using AutoMapper;
using Domain.Bid;
using Domain.Bid.Errors;
using Domain.Bid.Repository;
using Domain.Bid.Specification;
using FluentResults;
using MediatR;
using Shared.Domain.Interface;

namespace Application.MarkAuctionFinish;

public class MarkAuctionFinishCommandHandler(IBidRepository bidRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<MarkAuctionFinishCommand, Result<AuctionDto>>
{
    public async Task<Result<AuctionDto>> Handle(MarkAuctionFinishCommand request, CancellationToken cancellationToken)
    {
        var auctionByIdSpec = new AuctionByAuctionIdSpec(request.AuctionId);
        var auction = await bidRepository.GetAuctionById(auctionByIdSpec);
        if (auction is null) return Result.Fail<AuctionDto>(BidErrors.AuctionNotFound);

        var bidsByAuctionSpec = new BidsByAuctionSpec(request.AuctionId);
        var bids = await bidRepository.ListAsync(bidsByAuctionSpec, cancellationToken);

        var winningBid = bids.Where(p => p.BidStatus == BidStatus.Accepted).MaxBy(p => p.Amount);
        auction.FinishAuction(winningBid);
        await bidRepository.UpdateAuction(auction);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var auctionDto = mapper.Map<AuctionDto>(auction);
        if (winningBid is not null)
        {
            auctionDto.SoldAmount = winningBid.Amount;
        }
        return auctionDto;
    }
}