using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.UpdatePromoCode;

public class UpdatePromoCodeCommand : IRequest<BaseResponse>
{
    public Guid StoreId { get; set; }
    public Guid PromoCodeId { get; set; }
    public DateTime EndDate { get; set; }
    public int DiscountValue { get; set; }
    public int UsageLimit { get; set; }
    public bool IsActive { get; set; }
}