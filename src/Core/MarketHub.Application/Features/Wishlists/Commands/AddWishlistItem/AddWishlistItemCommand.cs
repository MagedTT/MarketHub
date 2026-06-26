using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Wishlists.Commands.AddWishlistItem;

public class AddWishlistItemCommand : IRequest<BaseResponse>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}