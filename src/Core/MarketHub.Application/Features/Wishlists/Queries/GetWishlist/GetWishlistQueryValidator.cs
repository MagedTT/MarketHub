using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Wishlists.Queries.GetWishlist;

public class GetWishlistQueryValidator : AbstractValidator<GetWishlistQuery>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetWishlistQueryValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is Required.")
            .MustAsync(UserExists)
            .WithMessage("User not found");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);
}