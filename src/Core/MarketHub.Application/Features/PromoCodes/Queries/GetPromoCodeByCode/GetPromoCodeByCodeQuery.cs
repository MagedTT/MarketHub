using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetPromoCodeByCode;

public class GetPromoCodeByCodeQuery : IRequest<GetPromoCodeByCodeQueryResponse>
{
    public string Code { get; set; } = string.Empty;
}