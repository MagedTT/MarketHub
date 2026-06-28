using FluentValidation;
using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Inventories.Commands.AddStock;

public class AddStockCommandValidator : AbstractValidator<AddStockCommand>
{
    public AddStockCommandValidator()
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