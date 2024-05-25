using FluentResults;
using MediatR;

namespace Application.PlaceBid;

public record PlaceBidCommand(string Bidder, int Amount, Guid AuctionId) : IRequest<Result<BidDto>>
{

}