using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.CheckValidPromoCodeByCode;

public class CheckValidPromoCodeByCodeCommandResponse : BaseResponse
{
    public decimal DiscountValue { get; set; }
    public CheckValidPromoCodeByCodeCommandResponse()
        : base()
    { }
}