using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetPromoCodeByCode;

public class GetPromoCodeByCodeQueryResponse : BaseResponse
{
    public PromoCode? PromoCode { get; set; }
    public GetPromoCodeByCodeQueryResponse()
        : base()
    { }
}