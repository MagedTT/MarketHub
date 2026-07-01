using MarketHub.Domain.Enums;

namespace MarketHub.Application.DTOs.Persistence.PromoCodes;

public class PromoCodeDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int UsageLimit { get; set; }
    public bool IsActive { get; set; }
}