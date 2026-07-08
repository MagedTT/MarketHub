using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.DeactivatePromoCode;

public class DeactivatePromoCodeCommandHandler : IRequestHandler<DeactivatePromoCodeCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public DeactivatePromoCodeCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(DeactivatePromoCodeCommand request, CancellationToken cancellationToken)
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

        promoCode.IsActive = false;

        await _repositoryManager.SaveAsync();

        return response;
    }
}