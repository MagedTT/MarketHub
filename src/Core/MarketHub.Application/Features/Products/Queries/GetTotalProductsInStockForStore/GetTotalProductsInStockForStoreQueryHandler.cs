using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsInStockForStore;

public class GetTotalProductsInStockForStoreQueryHandler : IRequestHandler<GetTotalProductsInStockForStoreQuery, GetTotalProductsInStockForStoreQueryResponse>
{
    private IRepositoryManager _repositoryManager;
    public GetTotalProductsInStockForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetTotalProductsInStockForStoreQueryResponse> Handle(GetTotalProductsInStockForStoreQuery request, CancellationToken cancellationToken)
    {
        GetTotalProductsInStockForStoreQueryResponse response = new();
        GetTotalProductsInStockForStoreQueryValidator validator = new(_repositoryManager);

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

        response.TotalProductsInStock = await _repositoryManager.ProductRepository.TotalProductsInStockByStoreIdAsync(request.StoreId);

        return response;
    }
}