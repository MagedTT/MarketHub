using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetPromoCodeById;

public class GetPromoCodeByIdQueryResponse : BaseResponse
{
    public PromoCode? PromoCode { get; set; }
    public GetPromoCodeByIdQueryResponse()
        : base()
    { }
}