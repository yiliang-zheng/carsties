using AutoMapper;
using Domain.Bid;
using Domain.Bid.Errors;
using Domain.Bid.Repository;
using Domain.Bid.Specification;
using FluentResults;
using FluentResults.Extensions;
using MediatR;
using Shared.Domain.Interface;
using Shared.Extensions.ResultExtensions;

namespace Application.PlaceBid;

public class PlaceBidCommandHandler(IBidRepository bidRepository, IUnitOfWork unitOfWork, IMapper mapper, IGrpcAuctionClient grpcClient) : IRequestHandler<PlaceBidCommand, Result<BidDto>>
{
    public async Task<Result<BidDto>> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
    {
        var result = await GetAuction(request)
            .Bind(auction => ValidateSeller(auction, request))
            .Bind(auction => GetExistBids(auction, cancellationToken))
            .Bind(tuple => CreateBid(tuple.auction, tuple.bids, request))
            .Tap(async bid => await SaveDatabase(bid, cancellationToken))
            .Bind(MapDto);

        return result;
    }

    private async Task<Result<Auction>> GetAuction(PlaceBidCommand request)
    {
        var auctionByIdSpec = new AuctionByAuctionIdSpec(request.AuctionId);
        var auction = await bidRepository.GetAuctionById(auctionByIdSpec);
        //auction not found
        if (auction is null)
        {
            auction = grpcClient.GetAuction(request.AuctionId);
            if (auction is null) return Result.Fail<Auction>(BidErrors.AuctionNotFound);
        }

        return Result.Ok(auction);
    }

    private Result<Auction> ValidateSeller(Auction auction, PlaceBidCommand request)
    {
        //same bidder and seller
        if (auction.Seller.Equals(request.Bidder)) return Result.Fail<Auction>(BidErrors.SameBidderAndSeller);
        return Result.Ok(auction);
    }

    private async Task<Result<(Auction auction, List<Bid> bids)>> GetExistBids(Auction auction, CancellationToken cancellationToken)
    {
        //get other bids of auction
        var bidsByAuctionIdSpec = new BidsByAuctionSpec(auction.Id);
        var currentBids = (await bidRepository.ListAsync(bidsByAuctionIdSpec, cancellationToken)) ?? [];
        return Result.Ok<(Auction auction, List<Bid> bids)>((auction, currentBids));
    }

    private Result<Bid> CreateBid(Auction auction, List<Bid> currentBids, PlaceBidCommand request)
    {
        var bid = Bid.Create(request.Bidder, DateTimeOffset.UtcNow, request.Amount, auction, currentBids);

        return Result.Ok(bid);
    }

    private async Task SaveDatabase(Bid bid, CancellationToken cancellationToken)
    {
        await bidRepository.AddAsync(bid, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private Result<BidDto> MapDto(Bid bid)
    {
        var dto = mapper.Map<BidDto>(bid);
        return Result.Ok(dto);
    }
}