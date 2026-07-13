using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Notifications.Commands.MarkAllNotificationAsReadForUser;

public class MarkAllNotificationAsReadForUserCommandValidator : AbstractValidator<MarkAllNotificationAsReadForUserCommand>
{
    private readonly IRepositoryManager _repositoryManager;

    public MarkAllNotificationAsReadForUserCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(UserExists)
            .WithMessage("User not found.");
    }

    public async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);
}