using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Product;
using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetProductDetails;

public class GetProductDetailsQueryHandler : IRequestHandler<GetProductDetailsQuery, GetProductDetailsQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetProductDetailsQueryHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<GetProductDetailsQueryResponse> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
    {
        GetProductDetailsQueryResponse response = new();
        GetProductDetailsQueryValidator validator = new();

        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = "product Id is null or empty";
            response.ValidationErrors = new();

            foreach (ValidationFailure failure in validationResult.Errors)
                response.ValidationErrors.Add($"{failure.PropertyName},{failure.ErrorMessage}");

            return response;
        }

        ProductDetailsDto? productDetails = await _repositoryManager.ProductRepository.GetProductDetailsByIdAsync(request.ProductId, request.TrackChanges);

        if (productDetails is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Product with Id: {request.ProductId} not found.";

            return response;
        }

        response.ProductDetailsDto = productDetails;

        return response;
    }
}