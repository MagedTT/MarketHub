using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Carts.Commands.DeleteCart;

public class DeleteCartCommandValidator : AbstractValidator<DeleteCartCommand>
{
    private readonly IRepositoryManager _repositoryManager;

    public DeleteCartCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Is is Required.")
            .MustAsync(UserExists)
            .WithMessage("User not found.");

        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("Cart Id is Required.");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);
}