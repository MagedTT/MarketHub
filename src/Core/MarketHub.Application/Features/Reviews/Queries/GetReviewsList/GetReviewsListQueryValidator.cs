using FluentValidation;

namespace MarketHub.Application.Features.Reviews.Queries.GetReviewsList;

public class GetReviewsListQueryValidator : AbstractValidator<GetReviewsListQuery>
{
    public GetReviewsListQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");
    }
}