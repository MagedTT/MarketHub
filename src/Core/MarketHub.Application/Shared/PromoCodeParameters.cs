using MarketHub.Domain.Enums;

namespace MarketHub.Application.Shared;

public class PromoCodeParameters : RequestParameters
{
    public Guid? StoreId { get; set; }
    public DiscountType? DiscountType { get; set; }
    public bool OrderByDiscountValueDescending { get; set; } = false;
    public DateTime StartDate { get; set; }
    public bool OrderByStartDateDescending { get; set; } = false;
    public DateTime EndDate { get; set; }
    public bool OrderByEndDateDescending { get; set; } = false;
    public bool? IsActive { get; set; }
    public int UsageLimitMin { get; set; } = 0;
    public int UsageLimitMax { get; set; } = 10000;
    public int? NumberOfTimesUsed { get; set; }
    public bool OrderByNumberOfTimesUsedDescending { get; set; } = false;
}