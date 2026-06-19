using MarketHub.Domain.Enums;

namespace MarketHub.Domain.Entities;

public class Report
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ReporterUserId { get; set; }
    public User ReporterUser { get; set; } = default!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public Guid? StoreId { get; set; }
    public Store? Store { get; set; } = default!;

    public string Description { get; set; } = string.Empty;
    public ReportStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}