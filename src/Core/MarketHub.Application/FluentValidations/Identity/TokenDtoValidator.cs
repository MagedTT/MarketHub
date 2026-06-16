using FluentValidation;
using MarketHub.Application.Models.Authentication;

namespace MarketHub.Application.FluentValidations.Identity;

public class TokenDtoValidator : AbstractValidator<TokenDto>
{
    public TokenDtoValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .NotNull()
            .WithMessage("Access Token is Required.");

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .NotNull()
            .WithMessage("Refresh Token is Required.");
    }
}