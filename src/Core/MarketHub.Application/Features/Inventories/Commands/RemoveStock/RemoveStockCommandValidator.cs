using FluentValidation;

namespace MarketHub.Application.Features.Inventories.Commands.RemoveStock;

public class RemoveStockCommandValidator : AbstractValidator<RemoveStockCommand>
{
    public RemoveStockCommandValidator()
    {
        RuleFor(x => x.InventoryId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .GreaterThan(0)
            .WithMessage("{PropertyName} should be greater than 0");
    }
}