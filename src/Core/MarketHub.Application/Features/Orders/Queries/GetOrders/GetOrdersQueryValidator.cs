using FluentValidation;
using MarketHub.Application.Shared;

namespace MarketHub.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        RuleFor(x => x.OrderParameters.OrderMinTotalPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum Price should be greater than 0");

        RuleFor(x => x.OrderParameters.OrderMaxTotalPrice)
            .LessThanOrEqualTo(100_000)
            .WithMessage("Maximum Price should be less than 100,000$");
    }
}