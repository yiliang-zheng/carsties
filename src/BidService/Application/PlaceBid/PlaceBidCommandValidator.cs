using FluentValidation;

namespace Application.PlaceBid;

public class PlaceBidCommandValidator : AbstractValidator<PlaceBidCommand>
{
    public PlaceBidCommandValidator()
    {
        RuleFor(p => p.AuctionId).NotNull().NotEmpty().WithMessage("Auction ID is required.");
        RuleFor(p => p.Bidder).NotNull().NotEmpty().WithMessage("Bidder is required.");
        RuleFor(p => p.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }
}