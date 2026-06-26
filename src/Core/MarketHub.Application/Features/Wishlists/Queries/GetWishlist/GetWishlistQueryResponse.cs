using MarketHub.Application.DTOs.Persistence.Wishlist;
using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Wishlists.Queries.GetWishlist;

public class GetWishlistQueryResponse : BaseResponse
{
    public WishlistDto? Wishlist { get; set; }

    public GetWishlistQueryResponse()
        : base()
    { }
}