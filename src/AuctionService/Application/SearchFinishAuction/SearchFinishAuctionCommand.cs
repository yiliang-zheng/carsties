using FluentResults;
using MediatR;

namespace Application.SearchFinishAuction;

public record SearchFinishAuctionCommand() : IRequest<Result<List<Guid>>>;