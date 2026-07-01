using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<BaseResponse>
{
    public Guid UserId { get; set; }
    public string? PromoCode { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
}