using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.PromoCodes.GetUsageCountForPromoCode;

public class GetUsageCountForPromoCodeQueryResponse : BaseResponse
{
    public int UsageCount { get; set; }
    public GetUsageCountForPromoCodeQueryResponse()
        : base()
    { }
}