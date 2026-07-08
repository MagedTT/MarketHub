using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsOutOfStockForStore;

public class GetTotalProductsOutOfStockForStoreQueryHandler : IRequestHandler<GetTotalProductsOutOfStockForStoreQuery, GetTotalProductsOutOfStockForStoreQueryResponse>
{
    private IRepositoryManager _repositoryManager;
    public GetTotalProductsOutOfStockForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetTotalProductsOutOfStockForStoreQueryResponse> Handle(GetTotalProductsOutOfStockForStoreQuery request, CancellationToken cancellationToken)
    {
        GetTotalProductsOutOfStockForStoreQueryResponse response = new();
        GetTotalProductsOutOfStockForStoreQueryValidator validator = new(_repositoryManager);

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

        response.TotalProductsOutOfStock = await _repositoryManager.ProductRepository.TotalProductsOutOfStockByStoreIdAsync(request.StoreId);

        return response;
    }
}