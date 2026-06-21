namespace MarketHub.Application.DTOs.Persistence.Carts;

public class CartProductDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string BaseImageUrl { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
}