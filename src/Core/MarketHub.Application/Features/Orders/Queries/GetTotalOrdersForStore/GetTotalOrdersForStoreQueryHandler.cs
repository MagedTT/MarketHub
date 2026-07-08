using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetTotalOrdersForStore;

public class GetTotalOrdersForStoreQueryHandler : IRequestHandler<GetTotalOrdersForStoreQuery, GetTotalOrdersForStoreQueryResponse>
{
    private IRepositoryManager _repositoryManager;
    public GetTotalOrdersForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetTotalOrdersForStoreQueryResponse> Handle(GetTotalOrdersForStoreQuery request, CancellationToken cancellationToken)
    {
        GetTotalOrdersForStoreQueryResponse response = new();
        GetTotalOrdersForStoreQueryValiator validator = new(_repositoryManager);

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

        response.TotalOrders = await _repositoryManager.OrdersRepository.TotalOrdersByStoreIdAsync(request.StoreId);

        return response;
    }
}