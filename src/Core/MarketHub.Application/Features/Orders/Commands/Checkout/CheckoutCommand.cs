using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Orders.Commands.Checkout;

public class CheckoutCommand : IRequest<BaseResponse>
{
    public Guid UserId { get; set; }
}