using System.Net;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Features.Brands.Queries.GetTotalBrandsForStore;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.ExtendPromoCodeValidity;

public class ExtendPromoCodeValidityCommandHandler : IRequestHandler<ExtendPromoCodeValidityCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public ExtendPromoCodeValidityCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(ExtendPromoCodeValidityCommand request, CancellationToken cancellationToken)
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

        if (request.UsageLimit <= promoCode.UsageLimit)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            response.Message = "Invalid Usage Limit";

            return response;
        }

        if (request.EndDate <= request.EndDate)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            response.Message = "Invalid End Date";

            return response;
        }

        promoCode.UsageLimit = request.UsageLimit;
        promoCode.EndDate = request.EndDate;

        await _repositoryManager.SaveAsync();

        return response;
    }
}