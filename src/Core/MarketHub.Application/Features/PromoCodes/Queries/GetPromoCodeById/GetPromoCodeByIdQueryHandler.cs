using System.Net;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetPromoCodeById;

public class GetPromoCodeByIdQueryHandler : IRequestHandler<GetPromoCodeByIdQuery, GetPromoCodeByIdQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetPromoCodeByIdQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetPromoCodeByIdQueryResponse> Handle(GetPromoCodeByIdQuery request, CancellationToken cancellationToken)
    {
        GetPromoCodeByIdQueryResponse response = new();

        PromoCode? promoCode = await _repositoryManager.PromoCodeRepository.GetByIdAsync(request.PromoCodeId);

        if (promoCode is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"PromoCode with Id: {request.PromoCodeId} is not found.";

            return response;
        }

        response.PromoCode = promoCode;

        return response;
    }
}