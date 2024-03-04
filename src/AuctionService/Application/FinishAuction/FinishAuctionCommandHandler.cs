using AutoMapper;
using Domain.Auction;
using Domain.Auction.Specification;
using FluentResults;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interface;

namespace Application.FinishAuction;

public class FinishAuctionCommandHandler(IRepository<Auction> repository, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<FinishAuctionCommand, Result<AuctionDto>>
{


    public async Task<Result<AuctionDto>> Handle(FinishAuctionCommand request, CancellationToken cancellationToken)
    {
        var idSpec = new AuctionByIdSpec(request.AuctionId);
        var auction = await repository.GetAsync(idSpec, cancellationToken);
        if (auction == null) return Result.Fail<AuctionDto>("auction not found");

        auction.FinishAuction(request.ItemSold, request.Amount, request.Winner);
        await repository.UpdateAsync(auction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<AuctionDto>(auction);
        
        return result;
    }
}