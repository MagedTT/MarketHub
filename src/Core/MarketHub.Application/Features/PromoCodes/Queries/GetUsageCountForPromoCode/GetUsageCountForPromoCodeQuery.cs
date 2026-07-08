using MediatR;

namespace MarketHub.Application.Features.PromoCodes.GetUsageCountForPromoCode;

public class GetUsageCountForPromoCodeQuery : IRequest<GetUsageCountForPromoCodeQueryResponse>
{
    public Guid PromoCodeId { get; set; }
}