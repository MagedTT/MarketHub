using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.ExtendPromoCodeValidity;

public class ExtendPromoCodeValidityCommand : IRequest<BaseResponse>
{
    public Guid PromoCodeId { get; set; }
    public DateTime EndDate { get; set; }
    public int UsageLimit { get; set; }
}