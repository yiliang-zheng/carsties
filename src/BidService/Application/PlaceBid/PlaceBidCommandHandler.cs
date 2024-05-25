using AutoMapper;
using Domain.Bid;
using Domain.Bid.Errors;
using Domain.Bid.Repository;
using Domain.Bid.Specification;
using FluentResults;
using MediatR;
using Shared.Domain.Events;
using Shared.Domain.Interface;

namespace Application.PlaceBid;

public class PlaceBidCommandHandler(IBidRepository bidRepository, IUnitOfWork unitOfWork, IMapper mapper, IGrpcAuctionClient grpcClient) : IRequestHandler<PlaceBidCommand, Result<BidDto>>
{
    public async Task<Result<BidDto>> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
    {
        var auctionByIdSpec = new AuctionByAuctionIdSpec(request.AuctionId);
        var auction = await bidRepository.GetAuctionById(auctionByIdSpec);
        //auction not found
        if (auction is null)
        {
            auction = grpcClient.GetAuction(request.AuctionId);
            if (auction is null) return Result.Fail<BidDto>(BidErrors.AuctionNotFound);
        }

        //same bidder and seller
        if (auction.Seller.Equals(request.Bidder)) return Result.Fail<BidDto>(BidErrors.SameBidderAndSeller);

        //get other bids of auction
        var bidsByAuctionIdSpec = new BidsByAuctionSpec(auction.Id);
        var currentBids = await bidRepository.ListAsync(bidsByAuctionIdSpec, cancellationToken);

        var bid = new Bid(Guid.NewGuid(), request.Bidder, DateTimeOffset.UtcNow, request.Amount, auction.Id);
        bid.SetAuction(auction);
        bid.SetBidStatus(currentBids);
        await bidRepository.AddAsync(bid, cancellationToken);

        bid.RegisterDomainEvent(new BidPlaced
        {
            BidId = bid.Id,
            Amount = bid.Amount,
            AuctionId = bid.AuctionId,
            Bidder = bid.Bidder,
            BidStatus = bid.BidStatus.Name,
            BidTime = bid.BidTime
        });
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = mapper.Map<BidDto>(bid);
        return dto;
    }
}