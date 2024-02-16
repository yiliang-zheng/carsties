using Domain.Auction;
using FluentResults;
using MediatR;

namespace Application.UpdateBid;

public record PlaceBidCommand(Guid AuctionId, BidStatus Status, int BidAmount) : IRequest<Result<AuctionDto>>
{

}