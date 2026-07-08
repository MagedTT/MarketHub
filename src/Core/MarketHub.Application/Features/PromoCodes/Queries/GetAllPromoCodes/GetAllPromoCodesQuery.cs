using MarketHub.Application.DTOs.Persistence.PromoCodes;
using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetAllPromoCodes;

public class GetAllPromoCodesQuery : IRequest<(MetaData metaData, IEnumerable<PromoCodeDto> promoCodes)>
{
    public PromoCodeParameters PromoCodeParameters { get; set; } = new();
    public bool TrackChanges { get; set; } = false;
}