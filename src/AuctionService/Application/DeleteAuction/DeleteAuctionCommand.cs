using FluentResults;
using MediatR;

namespace Application.DeleteAuction;

public record DeleteAuctionCommand(Guid Id) : IRequest<Result>;