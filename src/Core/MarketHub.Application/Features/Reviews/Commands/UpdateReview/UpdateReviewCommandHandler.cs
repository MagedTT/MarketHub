using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, BaseResponse>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    public UpdateReviewCommandHandler(IMapper mapper, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        UpdateReviewCommandValidator validator = new();

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

        Review? review = await _repositoryManager.ReviewRepository.GetReviewByUserIdAndProductIdAsync(request.CurrentUserId, request.ProductId);

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

        _mapper.Map(request, review);

        await _repositoryManager.SaveAsync();

        return response;
    }
}