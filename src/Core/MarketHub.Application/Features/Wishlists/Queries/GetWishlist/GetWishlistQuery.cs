using MediatR;

namespace MarketHub.Application.Features.Wishlists.Queries.GetWishlist;

public class GetWishlistQuery : IRequest<GetWishlistQueryResponse>
{
    public Guid UserId { get; set; }
    public bool TrackChanges { get; set; }
}