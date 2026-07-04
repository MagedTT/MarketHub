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
            .LessThanOrEqualTo(10_000)
            .WithMessage("Minimum Price should be less than 10,000$");
    }
}