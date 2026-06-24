using FluentValidation;

namespace MarketHub.Application.Features.Carts.Commands.UpdateCart;

public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
{
    public UpdateCartCommandValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");
    }
}