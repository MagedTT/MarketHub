using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Stores.Queries.GetStore;

public class GetStoreQueryHandler : IRequestHandler<GetStoreQuery, GetStoreQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetStoreQueryResponse> Handle(GetStoreQuery request, CancellationToken cancellationToken)
    {
        GetStoreQueryResponse response = new();
        GetStoreQueryValidator validator = new(_repositoryManager);

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

        StoreDto? store = await _repositoryManager.StoreRepository.GetStoreDtoByIdAsync(request.StoreId);

        if (store is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Store with Id: {request.StoreId} is not found.";
            return response;
        }

        response.Store = store;

        return response;
    }
}