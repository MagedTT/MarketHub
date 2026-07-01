using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Orders.Commands.Checkout;

public class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    public CheckoutCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(UserExists)
            .WithMessage("User is not found.");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);
}