using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetPromoCodeById;

public class GetPromoCodeByIdQuery : IRequest<GetPromoCodeByIdQueryResponse>
{
    public Guid PromoCodeId { get; set; }
}