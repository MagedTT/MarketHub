using System.Xml.Schema;
using FluentValidation;

namespace MarketHub.Application.Features.Orders.Queries.GetOrder;

public class GetOrderQueryValidator : AbstractValidator<GetOrderQuery>
{
    public GetOrderQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");

        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.");
    }
}