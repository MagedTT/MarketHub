using FluentValidation;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Wishlists.Commands.RemoveWishlistItem;

public class RemoveWishlistItemCommandValidator : AbstractValidator<RemoveWishlistItemCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public RemoveWishlistItemCommandValidator(IRepositoryManager repositoryManager)
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

        RuleFor(x => x.WishlistId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(WishlistExists)
            .WithMessage("Wishlist not found");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        => await _repositoryManager.ProductRepository.CheckProductExistsByIsAsync(productId);

    private async Task<bool> WishlistExists(RemoveWishlistItemCommand command, Guid wishlistId, CancellationToken cancellationToken)
    => await _repositoryManager.WishlistRepository.CheckWishlistExistsByUserIdAndWishlistIdAsync(command.UserId, wishlistId);
}