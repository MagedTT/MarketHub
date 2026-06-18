using FluentValidation;

namespace MarketHub.Application.Features.Products.Queries.GetProductDetails;

public class GetProductDetailsQueryValidator : AbstractValidator<GetProductDetailsQuery>
{
    public GetProductDetailsQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .NotEmpty()
            .WithMessage($"Proudct Id is Required.");
    }
}