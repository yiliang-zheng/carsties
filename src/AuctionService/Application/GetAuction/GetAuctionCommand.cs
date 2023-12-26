using FluentResults;
using MediatR;

namespace Application.GetAuction;

public record GetAuctionCommand(Guid Id) : IRequest<Result<AuctionDto>>;