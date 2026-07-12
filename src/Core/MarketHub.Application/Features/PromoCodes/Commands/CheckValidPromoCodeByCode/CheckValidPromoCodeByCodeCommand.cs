using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.CheckValidPromoCodeByCode;

public class CheckValidPromoCodeByCodeCommand : IRequest<CheckValidPromoCodeByCodeCommandResponse>
{
    public string Code { get; set; } = string.Empty;
}