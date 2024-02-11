using FluentResults;
using MediatR;

namespace Application.FinishAuction;

public record FinishAuctionCommand(Guid AuctionId, bool ItemSold, string Winner, string Seller, int? Amount) : IRequest<Result<AuctionDto>>;