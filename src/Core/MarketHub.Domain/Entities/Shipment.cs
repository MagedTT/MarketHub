using MarketHub.Domain.Enums;

namespace MarketHub.Domain.Entities;

public class Shipment
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; } = default!;

    public string Carrier { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public ShipmentStatus Status { get; set; }

    public DateTime ShippedAt { get; set; }
    public DateTime DeliveredAt { get; set; }

    public ICollection<ShipmentTracking> ShipmentTrackings { get; set; } = new List<ShipmentTracking>();
}