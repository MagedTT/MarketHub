using System.Net;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetPromoCodeByCode;

public class GetPromoCodeByCodeQueryHandler : IRequestHandler<GetPromoCodeByCodeQuery, GetPromoCodeByCodeQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetPromoCodeByCodeQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetPromoCodeByCodeQueryResponse> Handle(GetPromoCodeByCodeQuery request, CancellationToken cancellationToken)
    {
        GetPromoCodeByCodeQueryResponse response = new();

        PromoCode? promoCode = await _repositoryManager.PromoCodeRepository.GetByCodeAsync(request.Code);

        if (promoCode is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"PromoCode with code: {request.Code} is not found.";

            return response;
        }

        response.PromoCode = promoCode;

        return response;
    }
}