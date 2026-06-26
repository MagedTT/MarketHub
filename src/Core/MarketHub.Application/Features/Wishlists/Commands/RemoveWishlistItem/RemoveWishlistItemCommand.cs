using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Wishlists.Commands.RemoveWishlistItem;

public class RemoveWishlistItemCommand : IRequest<BaseResponse>
{
    public Guid UserId { get; set; }
    public Guid WishlistId { get; set; }
    public Guid ProductId { get; set; }
}