using MarketHub.Domain.Enums;

namespace MarketHub.Domain.Entities;

public class ShipmentTracking
{
    public Guid Id { get; set; }

    public Guid ShipmentId { get; set; }
    public Shipment Shipment { get; set; } = default!;

    public ShipmentStatus Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}