using MarketHub.Domain.Enums;

namespace MarketHub.Application.Shared;

public class PromoCodeParameters : RequestParameters
{
    public Guid? StoreId { get; set; }
    public DiscountType? DiscountType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsActive { get; set; }
    public int? NumberOfTimesUsed { get; set; }
    public int UsageLimitMin { get; set; } = 0;
    public int UsageLimitMax { get; set; } = 10000;
    public bool OrderByDiscountValue { get; set; } = false;
    public bool OrderByStartDate { get; set; } = false;
    public bool OrderByEndDate { get; set; } = false;
    public bool OrderByUsageLimit { get; set; }
    public bool OrderByNumberOfTimesUsed { get; set; }
    public bool Descending { get; set; } = false;
}