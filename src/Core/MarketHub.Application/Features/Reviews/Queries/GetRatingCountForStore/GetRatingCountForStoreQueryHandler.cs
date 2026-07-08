using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Queries.GetRatingCountForStore;

public class GetRatingCountForStoreQueryHandler : IRequestHandler<GetRatingCountForStoreQuery, GetRatingCountForStoreQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetRatingCountForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetRatingCountForStoreQueryResponse> Handle(GetRatingCountForStoreQuery request, CancellationToken cancellationToken)
    {
        GetRatingCountForStoreQueryResponse response = new();
        GetRatingCountForStoreQueryValidator validator = new(_repositoryManager);

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

        response.StoreRatingCount = await _repositoryManager.ReviewRepository.RatingCountByStoreIdAsync(request.StoreId);

        return response;
    }
}