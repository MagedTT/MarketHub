using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Products.GetTopNBestSellingProductsForStore;

public class GetTopNBestSellingProductsForStoreQueryHandler : IRequestHandler<GetTopNBestSellingProductsForStoreQuery, GetTopNBestSellingProductsForStoreQueryResponse>
{
    private IRepositoryManager _repositoryManager;
    public GetTopNBestSellingProductsForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetTopNBestSellingProductsForStoreQueryResponse> Handle(GetTopNBestSellingProductsForStoreQuery request, CancellationToken cancellationToken)
    {
        GetTopNBestSellingProductsForStoreQueryResponse response = new();
        GetTopNBestSellingProductsForStoreQueryValidator validator = new(_repositoryManager);

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

        response.Products = await _repositoryManager.ProductRepository.TopNBestSellingProductsByStoreIdAsync(request.StoreId, request.N);

        return response;
    }
}