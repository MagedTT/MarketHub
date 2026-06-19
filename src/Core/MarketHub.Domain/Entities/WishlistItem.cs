namespace MarketHub.Domain.Entities;

public class WishlistItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid WishlistId { get; set; }
    public Wishlist Wishlist { get; set; } = default!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
}