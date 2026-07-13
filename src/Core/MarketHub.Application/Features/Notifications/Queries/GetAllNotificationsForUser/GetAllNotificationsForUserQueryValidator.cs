using FluentValidation;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Notifications.Queries.GetAllNotificationsForUser;

public class GetAllNotificationsForUserQueryValidator : AbstractValidator<GetAllNotificationsForUserQuery>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetAllNotificationsForUserQueryValidator(IRepositoryManager repositoryManager)
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