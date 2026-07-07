using MarketHub.Domain.Enums;

namespace MarketHub.Domain.Entities;

public class PromoCode
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Code { get; set; } = string.Empty;

    public Guid StoreId { get; set; }
    public Store Store { get; set; } = default!;

    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; }

    public int UsageLimit { get; set; }
    public int NumberOfTimesUsed { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}