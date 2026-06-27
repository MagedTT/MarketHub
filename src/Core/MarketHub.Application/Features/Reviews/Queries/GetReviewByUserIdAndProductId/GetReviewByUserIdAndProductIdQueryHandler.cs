using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Review;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Queries.GetReviewByUserId;

public class GetReviewByUserIdQueryHandler : IRequestHandler<GetReviewByUserIdAndProductIdQuery, GetReviewByUserIdAndProductIdQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetReviewByUserIdQueryHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<GetReviewByUserIdAndProductIdQueryResponse> Handle(GetReviewByUserIdAndProductIdQuery request, CancellationToken cancellationToken)
    {
        GetReviewByUserIdAndProductIdQueryResponse response = new();
        GetReviewByUserIdAndProductIdQueryValidator validator = new(_repositoryManager);

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

        ReviewDto? review = await _repositoryManager.ReviewRepository.GetReviewDtoByUserIdAndProductIdAsync(request.UserId, request.ProductId, request.TrackChanges);

        if (review is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Review for productId: {request.ProductId} by UserId: {request.UserId} is not found";

            return response;
        }

        response.Review = review;

        return response;
    }
}