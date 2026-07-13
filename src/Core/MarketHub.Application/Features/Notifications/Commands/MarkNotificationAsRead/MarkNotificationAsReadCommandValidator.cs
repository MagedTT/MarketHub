using FluentValidation;
using MarketHub.Application.Contracts.Persistence;

namespace MarketHub.Application.Features.Notifications.Commands.MarkNotificationAsRead;

public class MarkNotificationAsReadCommandValidator : AbstractValidator<MarkNotificationAsReadCommand>
{
    private readonly IRepositoryManager _repositoryManager;

    public MarkNotificationAsReadCommandValidator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(UserExists)
            .WithMessage("User not found.");

        RuleFor(x => x.NotificationId)
            .NotEmpty()
            .WithMessage("{PropertyName} is Required.")
            .MustAsync(NotificationExists)
            .WithMessage("Notification not found.");
    }

    public async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        => await _repositoryManager.UserRepository.CheckUserExistsAsync(userId);

    public async Task<bool> NotificationExists(MarkNotificationAsReadCommand command, Guid notificationId, CancellationToken cancellationToken)
        => await _repositoryManager.NotificationRepository.NotificationExistsByIdAndUserIdAsync(notificationId, command.UserId);
}