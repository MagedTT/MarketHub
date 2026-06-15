using MarketHub.Domain.Enums;

namespace MarketHub.Domain.Entities;

public class OrderStatusHistory
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; } = default!;

    public Guid ChangedById { get; set; }
    public User User { get; set; } = default!;

    public OrderStatus Status { get; set; }

    public DateTime ChangedAt { get; set; }
}