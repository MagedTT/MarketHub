namespace MarketHub.Application.DTOs.Persistence.Carts;

public class CartItemDto
{
    public Guid CartItemId { get; set; }
    public CartProductDto Product { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal SubTotal { get; set; }
}