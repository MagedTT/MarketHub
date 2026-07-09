using MarketHub.Domain.Enums;

namespace MarketHub.Application.DTOs.Persistence.Orders;

public class StoreOrderDto
{
    public Guid OrderId { get; set; }
    public Guid StoreId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? PromoCode { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public StoreOrderShippingAddressDto ShippingAddress { get; set; } = default!;
    public ICollection<StoreOrderItemDto> OrderItems { get; set; } = new List<StoreOrderItemDto>();
}