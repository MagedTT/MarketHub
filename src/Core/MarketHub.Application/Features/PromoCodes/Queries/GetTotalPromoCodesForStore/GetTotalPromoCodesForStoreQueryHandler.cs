using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetTotalPromoCodesForStore;

public class GetTotalPromoCodesForStoreQueryHandler : IRequestHandler<GetTotalPromoCodesForStoreQuery, GetTotalPromoCodesForStoreQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetTotalPromoCodesForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetTotalPromoCodesForStoreQueryResponse> Handle(GetTotalPromoCodesForStoreQuery request, CancellationToken cancellationToken)
    {
        GetTotalPromoCodesForStoreQueryResponse response = new();
        GetTotalPromoCodesForStoreQueryValidator validator = new(_repositoryManager);

        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count > 0)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ValidationErrors = new();

            foreach (ValidationFailure validationFailure in validationResult.Errors)
                response.ValidationErrors.Add($"{validationFailure.PropertyName},{validationFailure.ErrorMessage}");

            return response;
        }

        response.TotalPromoCodes = await _repositoryManager.PromoCodeRepository.TotalPromoCodesByStoreIdAsync(request.StoreId);

        return response;
    }
}