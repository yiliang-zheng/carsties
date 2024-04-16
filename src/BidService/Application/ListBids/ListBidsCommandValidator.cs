using FluentValidation;

namespace Application.ListBids;

public sealed class ListBidsCommandValidator:AbstractValidator<ListBidsCommand>
{
    public ListBidsCommandValidator()
    {
        RuleFor(command => command.AuctionId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("Auction ID is required.");
    }
}