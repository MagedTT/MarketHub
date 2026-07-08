using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.DeletePromoCode;

public class DeletePromoCodeCommand : IRequest<BaseResponse>
{
    public Guid PromoCodeId { get; set; }
}