using MarketHub.Domain.Enums;

namespace MarketHub.Application.DTOs.Persistence.Orders;

public class StoreOrderStatusCount
{
    public OrderStatus OrderStatus { get; set; }
    public int Count { get; set; }
}