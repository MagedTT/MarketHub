using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.CheckValidPromoCode;

public class CheckValidPromoCodeCommand : IRequest<BaseResponse>
{
    public Guid PromoCodeId { get; set; }
}