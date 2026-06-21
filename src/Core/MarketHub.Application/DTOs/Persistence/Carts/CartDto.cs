namespace MarketHub.Application.DTOs.Persistence.Carts;

public class CartDto
{
    public Guid CartId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<CartItemDto> Items { get; set; } = new List<CartItemDto>();
}