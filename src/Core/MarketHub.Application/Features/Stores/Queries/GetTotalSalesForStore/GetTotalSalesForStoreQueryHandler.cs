using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Stores.Queries.GetTotalSalesForStore;

public class GetTotalSalesForStoreQueryHandler : IRequestHandler<GetTotalSalesForStoreQuery, GetTotalSalesForStoreQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetTotalSalesForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetTotalSalesForStoreQueryResponse> Handle(GetTotalSalesForStoreQuery request, CancellationToken cancellationToken)
    {
        GetTotalSalesForStoreQueryResponse response = new();
        GetTotalSalesForStoreQueryValidator validator = new(_repositoryManager);

        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ValidationErrors = new();

            foreach (ValidationFailure validationFailure in validationResult.Errors)
                response.ValidationErrors.Add($"{validationFailure.PropertyName},{validationFailure.ErrorMessage}");

            return response;
        }

        response.TotalSales = await _repositoryManager.StoreRepository.TotalSalesByStoreIdAsync(request.StoreId);

        return response;
    }
}