using FluentResults;
using MediatR;

namespace Application.ListBids;

public sealed record ListBidsCommand(Guid AuctionId) : IRequest<Result<List<BidDto>>>;