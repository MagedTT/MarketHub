using System.Net;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using MarketHub.Domain.Enums;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.CheckValidPromoCodeByCode;

public class CheckValidPromoCodeByCodeCommandHandler : IRequestHandler<CheckValidPromoCodeByCodeCommand, CheckValidPromoCodeByCodeCommandResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public CheckValidPromoCodeByCodeCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<CheckValidPromoCodeByCodeCommandResponse> Handle(CheckValidPromoCodeByCodeCommand request, CancellationToken cancellationToken)
    {
        CheckValidPromoCodeByCodeCommandResponse response = new();

        PromoCode? promoCode = await _repositoryManager.PromoCodeRepository.GetByCodeAsync(request.Code);

        if (promoCode is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"PromoCode with Id: {request.Code} is not found.";

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

        response.DiscountValue = promoCode.DiscountValue;

        return response;
    }
}