using FluentValidation;
using MediatR;

namespace MarketHub.Application.Features.Inventories.Queries.GetInventoryByProductId;

public class GetInventoryByProductIdQueryValidator : AbstractValidator<GetInventoryByProductIdQuery>
{
    public GetInventoryByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");
    }
}