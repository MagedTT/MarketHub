using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetOrderStatusCountForStore;

public class GetOrderStatusCountForStoreQueryHandler : IRequestHandler<GetOrderStatusCountForStoreQuery, GetOrderStatusCountForStoreQueryResponse>
{
    private IRepositoryManager _repositoryManager;
    public GetOrderStatusCountForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetOrderStatusCountForStoreQueryResponse> Handle(GetOrderStatusCountForStoreQuery request, CancellationToken cancellationToken)
    {
        GetOrderStatusCountForStoreQueryResponse response = new();
        GetOrderStatusCountForStoreQueryValidator validator = new(_repositoryManager);

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

        response.StoreOrderStatusCounts = await _repositoryManager.OrdersRepository.OrderStatusCountByStoreIdAsync(request.StoreId);

        return response;
    }
}