using MarketHub.Domain.Enums;

namespace MarketHub.Domain.Entities;

public class InventoryReservation
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public Guid InventoryId { get; set; }
    public Inventory Inventory { get; set; } = default!;

    public int Quantity { get; set; }

    public InventoryReservationStatus Status { get; set; } = InventoryReservationStatus.Active;

    public DateTime ReservedAt { get; set; } = DateTime.Now;

    public DateTime ExpiresAt { get; set; }
}