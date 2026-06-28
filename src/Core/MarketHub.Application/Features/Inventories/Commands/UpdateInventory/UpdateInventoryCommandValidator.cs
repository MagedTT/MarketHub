using FluentValidation;
using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Inventories.Commans.UpdateInventory;

public class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");

        RuleFor(x => x.AvailableQuantity)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("{PropertyName} should be greather than or equal to 0");

        RuleFor(x => x.ReservedQuantity)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("{PropertyName} should be greather than or equal to 0"); ;
    }
}