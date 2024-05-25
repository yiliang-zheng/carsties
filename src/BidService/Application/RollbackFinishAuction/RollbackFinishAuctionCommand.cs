using FluentResults;
using MediatR;

namespace Application.RollbackFinishAuction;

public record RollbackFinishAuctionCommand(Guid AuctionId) : IRequest<Result<AuctionDto>>;