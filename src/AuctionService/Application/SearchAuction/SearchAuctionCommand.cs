using FluentResults;
using MediatR;

namespace Application.SearchAuction;

public record SearchAuctionCommand(DateTime? From) : IRequest<Result<List<AuctionDto>>>;