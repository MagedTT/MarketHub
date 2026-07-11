using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsForStore;

public class GetProductDetailsForStoreQueryHandler : IRequestHandler<GetProductDetailsForStoreQuery, GetProductDetailsForStoreQueryResponse>
{
    private IRepositoryManager _repositoryManager;
    public GetProductDetailsForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetProductDetailsForStoreQueryResponse> Handle(GetProductDetailsForStoreQuery request, CancellationToken cancellationToken)
    {
        GetProductDetailsForStoreQueryResponse response = new();
        GetProductDetailsForStoreQueryValidator validator = new(_repositoryManager);

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

        response.ProductDetails = await _repositoryManager.ProductRepository.GetProductDetailsByStoreIdAsync(request.StoreId, request.ProductId);

        return response;
    }
}