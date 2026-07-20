using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, CreateReviewCommandResponse>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    public CreateReviewCommandHandler(IMapper mapper, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    public async Task<CreateReviewCommandResponse> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        CreateReviewCommandResponse response = new();
        CreateReviewCommandValidator validator = new(_repositoryManager);

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


        ///////////// Later Business Logic to be added /////////////

        ////////////////////////////////////////////////////////////
        //////// Reviews should be after order is delivered ////////
        ////////////////////////////////////////////////////////////

        Product product = (await _repositoryManager.ProductRepository.GetByIdAsync(request.ProductId, true))!;

        decimal ratingsSum = product.AverageRating * product.NumberOfReviews + request.Rating;

        product.NumberOfReviews++;
        product.AverageRating = ratingsSum / product.NumberOfReviews;

        Review review = _mapper.Map<Review>(request);

        ReviewDto reviewToReturn = new()
        {
            Id = review.Id,
            ReviewerId = request.UserId,
            ProductId = request.ProductId,
            ReviewerRating = request.Rating,
            CreatedAt = review.CreatedAt,
            Comment = request.Comment
        };

        _repositoryManager.ReviewRepository.CreateReview(review);

        await _repositoryManager.SaveAsync();

        response.Review = reviewToReturn;

        return response;
    }
}