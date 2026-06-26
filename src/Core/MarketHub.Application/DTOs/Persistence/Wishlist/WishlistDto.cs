namespace MarketHub.Application.DTOs.Persistence.Wishlist;

public class WishlistDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ICollection<WishlistItemDto>? WishlistItems { get; set; }
}