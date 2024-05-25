using FluentResults;
using MediatR;

namespace Application.MarkAuctionFinish;

public record MarkAuctionFinishCommand(Guid AuctionId) : IRequest<Result<AuctionDto>>
{

}