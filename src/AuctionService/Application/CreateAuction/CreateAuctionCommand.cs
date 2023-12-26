using FluentResults;
using MediatR;

namespace Application.CreateAuction;

public record CreateAuctionCommand(
    string Make,
    string Model,
    string Color,
    int Mileage,
    int Year,
    int ReservePrice,
    string ImageUrl,
    DateTime AuctionEnd,
    string Seller
) : IRequest<Result<AuctionDto>>;
