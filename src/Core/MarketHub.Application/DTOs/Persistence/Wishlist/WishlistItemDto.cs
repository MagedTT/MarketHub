namespace MarketHub.Application.DTOs.Persistence.Wishlist;

public class WishlistItemDto
{
    public Guid Id { get; set; }
    // public Guid WishlistId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductBaseImageUrl { get; set; } = string.Empty;
    public decimal ProductUnitPrice { get; set; }
    public bool ProductInStock { get; set; }
}