using MediatR;

namespace MarketHub.Application.Features.PromoCodes.GetUsageCountForPromoCodeByCode;

public class GetUsageCountForPromoCodeByCodeQuery : IRequest<GetUsageCountForPromoCodeByCodeQueryResponse>
{
    public string Code { get; set; } = string.Empty;
}