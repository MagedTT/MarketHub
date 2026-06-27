using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, BaseResponse>
{
    private IRepositoryManager _repositoryManager;
    public DeleteReviewCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        DeleteReviewCommandValidator validator = new();

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

        Review? review = await _repositoryManager.ReviewRepository.GetReviewByIdAsync(request.ReviewId);

        if (review is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Review with Id: {request.ReviewId} is not found";

            return response;
        }

        if (review.UserId != request.CurrentUserId)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.Forbidden;
            response.Message = "You are not authorized to delete this review.";

            return response;
        }

        _repositoryManager.ReviewRepository.DeleteReview(review);

        await _repositoryManager.SaveAsync();

        return response;
    }
}