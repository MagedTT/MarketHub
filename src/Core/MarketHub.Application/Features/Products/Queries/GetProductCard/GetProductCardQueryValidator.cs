using FluentValidation;

namespace MarketHub.Application.Features.Products.Queries.GetProductCard;

public class GetProductCardQueryValidator : AbstractValidator<GetProductCardQuery>
{
    public GetProductCardQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product Id is Required.");
    }
}