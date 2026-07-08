using MarketHub.Application.Responses;
using MarketHub.Domain.Enums;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.CreatePromoCode;

public class CreatePromoCodeCommand : IRequest<BaseResponse>
{
    public Guid StoreId { get; set; }
    public string Code { get; set; } = string.Empty;
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTime EndDate { get; set; }
    public int UsageLimit { get; set; }
    public bool IsActive { get; set; }
}