namespace MarketHub.Application.DTOs.Persistence.Orders;

public class StoreOrderItemDto
{
    public Guid OrderItemId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductBaseImageUrl { get; set; } = string.Empty;
    public int ProductQuantity { get; set; }
    public decimal ProductUnitPrice { get; set; }
}