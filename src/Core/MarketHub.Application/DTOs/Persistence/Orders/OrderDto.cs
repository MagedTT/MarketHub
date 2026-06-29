using MarketHub.Domain.Enums;
using MediatR.Pipeline;

namespace MarketHub.Application.DTOs.Persistence.Orders;

public class OrderDto
{
    public Guid Id { get; set; }
    public string OrderedByUserName { get; set; } = string.Empty;
    public int NumberOfOrderedProducts { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DateOfDelivery { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string? PromoCode { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}