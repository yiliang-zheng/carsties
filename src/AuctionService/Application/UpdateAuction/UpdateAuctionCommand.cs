using FluentResults;
using MediatR;

namespace Application.UpdateAuction;

public record UpdateAuctionCommand(
    Guid Id,
    string Make,
    string Model,
    string Color,
    int? Mileage,
    int? Year,
    string Seller
    ) : IRequest<Result<AuctionDto>>;