using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.DeactivatePromoCode;

public class DeactivatePromoCodeCommand : IRequest<BaseResponse>
{
    public Guid PromoCodeId { get; set; }
}