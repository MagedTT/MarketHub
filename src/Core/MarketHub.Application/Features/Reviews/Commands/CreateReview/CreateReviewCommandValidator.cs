using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public CreateReviewCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(UserExists)
            .WithMessage("User is not found.");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(ProductExists)
            .WithMessage("Product is not found.")
            .MustAsync(ReviewDoesntExist)
            .WithMessage("A Review for that product already exists");

        RuleFor(x => x.Rating)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Rating should be between 0 and 5")
            .LessThanOrEqualTo(5)
            .WithMessage("Rating should be between 0 and 5");

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Comment length should be less than 500 characters.");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        => await _repositoryManager.ProductRepository.CheckProductExistsByIsAsync(productId);

    private async Task<bool> ReviewDoesntExist(CreateReviewCommand command, Guid productId, CancellationToken cancellationToken)
        => !await _repositoryManager.ReviewRepository.ReviewExists(command.UserId, productId);
}