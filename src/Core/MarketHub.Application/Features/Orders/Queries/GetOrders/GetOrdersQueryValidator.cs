using FluentValidation;
using MarketHub.Application.Shared;

namespace MarketHub.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        RuleFor(x => x.OrderParameters.OrderByCreationTimeDescending)
            .NotEmpty()
            .WithMessage("Ordering Direction is Required");

        RuleFor(x => x.OrderParameters.OrderMinTotalPrice)
            .NotEmpty()
            .WithMessage("Order Minimum Total Price is Required")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum Price should be greater than 0");

        RuleFor(x => x.OrderParameters.OrderMaxTotalPrice)
            .NotEmpty()
            .WithMessage("Order Maximum Total Price is Required")
            .LessThanOrEqualTo(10_000)
            .WithMessage("Minimum Price should be less than 10,000$");
    }
}