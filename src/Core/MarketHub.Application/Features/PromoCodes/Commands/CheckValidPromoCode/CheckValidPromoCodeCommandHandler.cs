using System.Net;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MarketHub.Domain.Enums;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.CheckValidPromoCode;

public class CheckValidPromoCodeCommandHandler : IRequestHandler<CheckValidPromoCodeCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public CheckValidPromoCodeCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(CheckValidPromoCodeCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();

        PromoCode? promoCode = await _repositoryManager.PromoCodeRepository.GetByIdAsync(request.PromoCodeId);

        if (promoCode is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"PromoCode with Id: {request.PromoCodeId} is not found.";

            return response;
        }

        if (
            promoCode.EndDate <= DateTime.Now ||
            promoCode.UsageLimit <= promoCode.NumberOfTimesUsed ||
            !promoCode.IsActive ||
            promoCode.DiscountType == DiscountType.FixedAmount)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            response.Message = "Invalid Promo Code";
            return response;
        }

        return response;
    }
}