namespace MarketHub.Domain.Entities;

public class Wishlist
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
}