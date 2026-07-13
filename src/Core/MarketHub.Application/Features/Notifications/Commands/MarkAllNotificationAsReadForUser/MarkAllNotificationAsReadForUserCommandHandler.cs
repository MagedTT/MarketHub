using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Notifications.Commands.MarkAllNotificationAsReadForUser;

public class MarkAllNotificationAsReadForUserCommandHandler : IRequestHandler<MarkAllNotificationAsReadForUserCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public MarkAllNotificationAsReadForUserCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(MarkAllNotificationAsReadForUserCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();

        MarkAllNotificationAsReadForUserCommandValidator validator = new(_repositoryManager);

        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ValidationErrors = new();

            foreach (ValidationFailure validationFailure in validationResult.Errors)
                response.ValidationErrors.Add($"{validationFailure.PropertyName},{validationFailure.ErrorMessage}");

            return response;
        }

        await _repositoryManager.NotificationRepository.MarkAllNotificationsAsReadByUserIdAsync(request.UserId);

        return response;
    }
}