using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Orders;
using MarketHub.Application.Shared;
using MarketHub.Domain.Enums;
using MediatR;

namespace MarketHub.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, GetOrdersQueryResponse>
{
    private IRepositoryManager _repositoryManager;
    public GetOrdersQueryHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<GetOrdersQueryResponse> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        GetOrdersQueryResponse response = new();
        GetOrdersQueryValidator validator = new();

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

        PagedList<OrderDto> ordersDtosWithMetaData = await _repositoryManager.OrdersRepository.GetOrdersAsync(request.OrderParameters, request.TrackChanges);

        response.Orders = ordersDtosWithMetaData.AsEnumerable();
        response.MetaData = ordersDtosWithMetaData.MetaData;

        return response;
    }
}