using FastEndpoints;
using FluentValidation;
using Shared.Constants;

namespace WebApi.Auction;

public class CreateAuctionRequestValidator:Validator<CreateAuctionRequest>
{
    public CreateAuctionRequestValidator()
    {
        RuleFor(p => p.Make)
            .NotEmpty()
            .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
            .WithMessage("Make is required");

        RuleFor(p => p.Model)
            .NotEmpty()
            .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
            .WithMessage("Model is required");

        RuleFor(p => p.Color)
            .NotEmpty()
            .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
            .WithMessage("Color is required");

        RuleFor(p => p.ImageUrl)
            .NotEmpty()
            .MaximumLength(DataSchemaConstants.DEFAULT_URL_LENGHT)
            .WithMessage("Image URL is required");

    }
}