using AutoMapper;
using Domain.Auction;
using Domain.Auction.Errors;
using Domain.Auction.Specification;
using FluentResults;
using MediatR;
using Shared.Domain.Interface;

namespace Application.RollbackFinishAuction;

public class RollbackFinishAuctionCommandHandler(IRepository<Auction> auctionRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<RollbackFinishAuctionCommand, Result<AuctionDto>>
{
    /// <summary>
    /// rollback auction to live
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<AuctionDto>> Handle(RollbackFinishAuctionCommand request, CancellationToken cancellationToken)
    {
        var auctionByIdSpec = new AuctionByIdSpec(request.AuctionId);
        var auction = await auctionRepository.GetAsync(auctionByIdSpec, cancellationToken);
        if (auction is null)
        {
            return Result.Fail<AuctionDto>(AuctionErrors.AuctionNotFound);
        }

        auction.UpdateStatus(Status.Live);
        await auctionRepository.UpdateAsync(auction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = mapper.Map<AuctionDto>(auction);
        return dto;
    }
}