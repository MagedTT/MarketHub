using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetOrderForStore;

public class GetOrderForStoreQueryHandler : IRequestHandler<GetOrderForStoreQuery, GetOrderForStoreQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetOrderForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetOrderForStoreQueryResponse> Handle(GetOrderForStoreQuery request, CancellationToken cancellationToken)
    {
        GetOrderForStoreQueryResponse response = new();

        GetOrderForStoreQueryValidator validator = new(_repositoryManager);

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

        response.Order = await _repositoryManager.OrdersRepository.GetOrderByOrderIdAndStoreIdAsync(request.OrderId, request.StoreId);

        return response;
    }
}