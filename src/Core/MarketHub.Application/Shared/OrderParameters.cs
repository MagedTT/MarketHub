using MarketHub.Domain.Enums;

namespace MarketHub.Application.Shared;

public class OrderParameters : RequestParameters
{
    public Guid? UserId { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public bool OrderByCreationTimeDescending { get; set; } = false;
    public decimal OrderMinTotalPrice { get; set; } = 0;
    public decimal OrderMaxTotalPrice { get; set; } = 100_000;
}