using FastEndpoints;
using FluentValidation;

namespace WebApi.Auction;

public class DeleteAuctionRequestValidator : Validator<DeleteAuctionRequest>
{
    public DeleteAuctionRequestValidator()
    {
        RuleFor(p => p.Id)
            .NotNull()
            .WithMessage("Id is required.")
            .NotEqual(default(Guid))
            .WithMessage("Id is required.");
    }
}