using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.PromoCodes.GetUsageCountForPromoCodeByCode;

public class GetUsageCountForPromoCodeByCodeQueryResponse : BaseResponse
{
    public int UsageCount { get; set; }
    public GetUsageCountForPromoCodeByCodeQueryResponse()
        : base()
    { }
}