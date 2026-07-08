using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetTotalProductsForStore;

public class GetTotalProductsForStoreQueryHandler : IRequestHandler<GetTotalProductsForStoreQuery, GetTotalProductsForStoreQueryResponse>
{
    private IRepositoryManager _repositoryManager;
    public GetTotalProductsForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetTotalProductsForStoreQueryResponse> Handle(GetTotalProductsForStoreQuery request, CancellationToken cancellationToken)
    {
        GetTotalProductsForStoreQueryResponse response = new();
        GetTotalProductsForStoreQueryValidator validator = new(_repositoryManager);

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

        response.TotalProducts = await _repositoryManager.ProductRepository.TotalProductsByStoreIdAsync(request.StoreId);

        return response;
    }
}