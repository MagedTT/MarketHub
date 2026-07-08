using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Queries.GetTotalReviewsForStore;

public class GetTotalReviewsForStoreQueryHandler : IRequestHandler<GetTotalReviewsForStoreQuery, GetTotalReviewsForStoreQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetTotalReviewsForStoreQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetTotalReviewsForStoreQueryResponse> Handle(GetTotalReviewsForStoreQuery request, CancellationToken cancellationToken)
    {
        GetTotalReviewsForStoreQueryResponse response = new();
        GetTotalReviewsForStoreQueryValidator validator = new(_repositoryManager);

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

        response.TotalReviews = await _repositoryManager.ReviewRepository.TotalReviewsByStoreIdAsync(request.StoreId);

        return response;
    }
}