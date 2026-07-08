using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetRecentOrdersForStore;

public class GetRecentOrdersForStoreCommandHandler : IRequestHandler<GetRecentOrdersForStoreCommand, GetRecentOrdersForStoreCommandResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetRecentOrdersForStoreCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetRecentOrdersForStoreCommandResponse> Handle(GetRecentOrdersForStoreCommand request, CancellationToken cancellationToken)
    {
        GetRecentOrdersForStoreCommandResponse response = new();
        GetRecentOrdersForStoreCommandValidator validator = new(_repositoryManager);

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

        PagedList<StoreOrderDto> ordersWithMetaData = await _repositoryManager.OrdersRepository.GetRecentOrdersByStoreIdAsync(request.StoreId, request.StoreOrdersParameters);

        response.MetaData = ordersWithMetaData.MetaData;
        response.Orders = ordersWithMetaData.AsEnumerable();

        return response;
    }
}