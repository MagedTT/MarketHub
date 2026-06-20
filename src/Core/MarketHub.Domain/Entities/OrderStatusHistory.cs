using MarketHub.Domain.Enums;

namespace MarketHub.Domain.Entities;

public class OrderStatusHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OrderId { get; set; }
    public Order Order { get; set; } = default!;

    public Guid? ChangedByUserId { get; set; }
    public User? ChangedByUser { get; set; } = default!;

    public OrderStatus Status { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.Now;
}