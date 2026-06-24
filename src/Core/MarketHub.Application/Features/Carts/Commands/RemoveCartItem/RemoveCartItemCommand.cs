using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Carts.Commands.RemoveCartItem;

public class RemoveCartItemCommand : IRequest<BaseResponse>
{
    public Guid CartId { get; set; }
    public Guid CartItemId { get; set; }
}