using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Product;
using MediatR;

namespace MarketHub.Application.Features.Brands.Queries.GetTotalBrandsForStore;

public class GetTopNSellingBrandsForStoreQueryHandler : IRequestHandler<GetTopNSellingBrandsForStoreQuery, GetTopNSellingBrandsForStoreQueryResponse>
{
    private IRepositoryManager _repositoryManager;
    public GetTopNSellingBrandsForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetTopNSellingBrandsForStoreQueryResponse> Handle(GetTopNSellingBrandsForStoreQuery request, CancellationToken cancellationToken)
    {
        GetTopNSellingBrandsForStoreQueryResponse response = new();
        GetTopNSellingBrandsForStoreQueryValidator validator = new(_repositoryManager);

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

        response.Brands = await _repositoryManager.BrandRepository.TopNBestSellingBrandsByStoreIdAsync(request.StoreId, request.N);

        return response;
    }
}
