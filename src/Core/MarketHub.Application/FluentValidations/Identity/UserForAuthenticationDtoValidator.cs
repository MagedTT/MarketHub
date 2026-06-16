using FluentValidation;
using MarketHub.Application.Models.Authentication;

namespace MarketHub.Application.FluentValidations.Identity;

public class UserForAuthenticationDtoValidator : AbstractValidator<UserForAuthenticationDto>
{
    public UserForAuthenticationDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email is Required.")
            .EmailAddress()
            .WithMessage("Please Provide a Valid Email Address.");

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password is Required.")
            .MinimumLength(8)
            .WithMessage("Minimum Length is 8 Characters.")
            .Matches("[A-Z]").WithMessage("Password Must Contain at least One Uppercase Letter.")
            .Matches("[a-z]").WithMessage("Password Must Contain at least One Lowercase Letter.")
            .Matches("[0-9]").WithMessage("Password Must Contain at least One Digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password Must Contain at least One Special Character.")
            .Must(x => x.Distinct().Count() >= 3)
            .WithMessage("Password Must Contains at least 3 Unique Characters.");
    }
}