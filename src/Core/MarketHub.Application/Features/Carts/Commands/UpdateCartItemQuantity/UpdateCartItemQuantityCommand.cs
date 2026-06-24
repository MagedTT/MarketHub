using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Carts.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommand : IRequest<BaseResponse>
{
    public Guid CartItemId { get; set; }
    // public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}