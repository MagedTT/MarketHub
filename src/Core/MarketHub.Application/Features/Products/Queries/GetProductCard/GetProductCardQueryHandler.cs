using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Product;
using MediatR;

namespace MarketHub.Application.Features.Products.Queries.GetProductCard;

public class GetProductCardQueryHandler : IRequestHandler<GetProductCardQuery, GetProductCardQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetProductCardQueryHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<GetProductCardQueryResponse> Handle(GetProductCardQuery request, CancellationToken cancellationToken)
    {
        GetProductCardQueryResponse response = new();
        GetProductCardQueryValidator validator = new();

        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count > 0)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = "Product Id is null or empty";

            response.ValidationErrors = new List<string>();

            foreach (ValidationFailure validationFailure in validationResult.Errors)
                response.ValidationErrors.Add($"{validationFailure.PropertyName},{validationFailure.ErrorMessage}");

            return response;
        }

        ProductCardDto? productCardDto = await _repositoryManager.ProductRepository.GetProductCardByIdAsync(request.ProductId, request.TrackChanges);

        if (productCardDto is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Product with Id: {request.ProductId} is not found.";

            return response;
        }

        response.ProductCardDto = productCardDto;

        return response;
    }
}