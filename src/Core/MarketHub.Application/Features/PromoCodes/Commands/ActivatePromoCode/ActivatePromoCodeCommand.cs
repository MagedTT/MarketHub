using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.ActivatePromoCode;

public class ActivatePromoCodeCommand : IRequest<BaseResponse>
{
    public Guid PromoCodeId { get; set; }
}