using FastEndpoints;
using FluentValidation;

namespace WebApi.Auction;

public class GetAuctionRequestValidator : Validator<GetAuctionRequest>
{
    public GetAuctionRequestValidator()
    {
        RuleFor(p => p.Id)
            .NotEqual(default(Guid))
            .WithMessage("Id is required");
    }
}