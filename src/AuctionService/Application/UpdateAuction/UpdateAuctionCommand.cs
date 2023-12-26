using FluentResults;
using MediatR;

namespace Application.UpdateAuction;

public record UpdateAuctionCommand(
    Guid Id,
    string Make,
    string Model,
    string Color,
    int? Mileage,
    int? Year
    ) : IRequest<Result<AuctionDto>>;