using FluentValidation;

namespace IdentityService.Pages.Account.Register;

public class InputModelValidator : AbstractValidator<InputModel>
{
    public InputModelValidator()
    {
        RuleFor(p => p.Email)
            .NotNull().NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(p => p.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password is required.");

        RuleFor(p => p.ConfirmedPassword)
            .NotNull()
            .NotEmpty()
            .WithMessage("Confirmed password is required.")
            .Equal(p => p.Password)
            .WithMessage("Confirmed password must be the same as password.");

        RuleFor(p => p.Username)
            .NotNull()
            .NotEmpty()
            .WithMessage("Username is required.");

        RuleFor(p => p.FullName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Full name is required.");
    }
}