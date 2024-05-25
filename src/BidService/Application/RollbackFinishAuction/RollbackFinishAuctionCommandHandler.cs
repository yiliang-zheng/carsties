using AutoMapper;
using Domain.Bid;
using Domain.Bid.Errors;
using Domain.Bid.Repository;
using Domain.Bid.Specification;
using FluentResults;
using MediatR;
using Shared.Domain.Interface;

namespace Application.RollbackFinishAuction;

public class RollbackFinishAuctionCommandHandler(IBidRepository bidRepository, IUnitOfWork unitOfWork, IMapper mapper):IRequestHandler<RollbackFinishAuctionCommand, Result<AuctionDto>>
{
    public async Task<Result<AuctionDto>> Handle(RollbackFinishAuctionCommand request, CancellationToken cancellationToken)
    {
        var auctionByIdSpec = new AuctionByAuctionIdSpec(request.AuctionId);
        var auction = await bidRepository.GetAuctionById(auctionByIdSpec);
        if (auction is null)
        {
            return Result.Fail<AuctionDto>(BidErrors.AuctionNotFound);
        }

        auction.RollbackFinishAuction();
        await bidRepository.UpdateAuction(auction);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = mapper.Map<AuctionDto>(auction);
        return dto;
    }
}