using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Queries.GetReviewsList;

public class GetReviewsListQueryHandler : IRequestHandler<GetReviewsListQuery, GetReviewsListQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetReviewsListQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetReviewsListQueryResponse> Handle(GetReviewsListQuery request, CancellationToken cancellationToken)
    {
        GetReviewsListQueryResponse response = new();
        GetReviewsListQueryValidator validator = new();

        ValidationResult result = await validator.ValidateAsync(request);

        if (!result.IsValid)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ValidationErrors = new();

            foreach (ValidationFailure validationFailure in result.Errors)
                response.ValidationErrors.Add($"{validationFailure.PropertyName},{validationFailure.ErrorMessage}");

            return response;
        }

        PagedList<ReviewDto> reviews = await _repositoryManager.ReviewRepository.GetReviewsForProductWithIdAsync(request.ProductId, request.RequestParameters, request.TrackChanges);

        response.Reviews = reviews.AsEnumerable();
        response.MetaData = reviews.MetaData;

        return response;
    }
}