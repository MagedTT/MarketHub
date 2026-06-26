using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Wishlists.Commands.AddWishlistItem;

public class AddWishlistItemCommandValidator : AbstractValidator<AddWishlistItemCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public AddWishlistItemCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(UserExists)
            .WithMessage("User not found");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(ProductExists)
            .WithMessage("User not found");

        // RuleFor(x => x.WishlistId)
        //     .NotEmpty()
        //     .WithMessage("{PropertyName} is Required.");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        => await _repositoryManager.ProductRepository.CheckProductExistsByIsAsync(productId);
}