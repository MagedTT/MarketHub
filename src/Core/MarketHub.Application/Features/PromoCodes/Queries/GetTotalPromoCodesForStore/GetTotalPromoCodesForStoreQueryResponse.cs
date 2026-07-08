using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetTotalPromoCodesForStore;

public class GetTotalPromoCodesForStoreQueryResponse : BaseResponse
{
    public int TotalPromoCodes { get; set; }

    public GetTotalPromoCodesForStoreQueryResponse()
        : base()
    { }
}