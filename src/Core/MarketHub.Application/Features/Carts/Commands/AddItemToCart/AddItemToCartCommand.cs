using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Carts.Commands.AddItemToCart;

public class AddItemToCartCommand : IRequest<BaseResponse>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}