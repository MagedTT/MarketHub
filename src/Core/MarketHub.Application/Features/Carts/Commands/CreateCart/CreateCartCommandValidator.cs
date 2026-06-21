using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Carts.Commands.CreateCart;

public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public CreateCartCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User is required.")
            .MustAsync(UserExists)
            .WithMessage("User does not exist.")
            .MustAsync(CartDoesNotExist)
            .WithMessage("User already has a cart.");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);

    private async Task<bool> CartDoesNotExist(Guid userId, CancellationToken cancellationToken)
        => !await _repositoryManager.CartRepository.CartExistsAsync(userId);

}