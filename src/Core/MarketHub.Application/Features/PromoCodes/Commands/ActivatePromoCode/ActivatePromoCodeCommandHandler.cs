using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.ActivatePromoCode;

public class ActivatePromoCodeCommandHandler : IRequestHandler<ActivatePromoCodeCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public ActivatePromoCodeCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(ActivatePromoCodeCommand request, CancellationToken cancellationToken)
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

        if (promoCode.EndDate <= DateTime.Now)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"PromoCode with Id: {request.PromoCodeId} is expired.";

            return response;
        }

        if (promoCode.NumberOfTimesUsed >= promoCode.UsageLimit)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"PromoCode with Id: {request.PromoCodeId} has passed the usage limit.";

            return response;
        }

        promoCode.IsActive = true;

        await _repositoryManager.SaveAsync();

        return response;
    }
}