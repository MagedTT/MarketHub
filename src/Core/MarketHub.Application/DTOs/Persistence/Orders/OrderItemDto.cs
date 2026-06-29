using System.Text.Json;

namespace MarketHub.Application.DTOs.Persistence.Orders;

public class OrderItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductBaseImageUrl { get; set; } = string.Empty;
    public JsonElement ProductSpecifications { get; set; }
}