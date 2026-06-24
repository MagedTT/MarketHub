using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Carts.Commands.UpdateCart;

public class UpdateCartCommand : IRequest<BaseResponse>
{
    public Guid CartId { get; set; }
    public IEnumerable<Guid>? CartItemsIds { get; set; }
}