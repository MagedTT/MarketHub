using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetAllProductsForStore;

public class GetAllProductsForStoreQueryHandler : IRequestHandler<GetAllProductsForStoreQuery, GetAllProductsForStoreQueryResponse>
{
    private IRepositoryManager _repositoryManager;
    public GetAllProductsForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetAllProductsForStoreQueryResponse> Handle(GetAllProductsForStoreQuery request, CancellationToken cancellationToken)
    {
        GetAllProductsForStoreQueryResponse response = new();
        GetAllProductsForStoreQueryValidator validator = new(_repositoryManager);

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

        PagedList<ProductDto> productsWithMetaData = await _repositoryManager.ProductRepository.GetAllProductsByStoreIdAsync(request.StoreId, request.StoreProductParameters);

        response.MetaData = productsWithMetaData.MetaData;
        response.Products = productsWithMetaData.AsEnumerable();

        return response;
    }
}