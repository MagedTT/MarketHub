using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Models.Notification;
using MediatR;

namespace MarketHub.Application.Features.Notifications.Queries.GetAllNotificationsForUser;

public class GetAllNotificationsForUserQueryHandler : IRequestHandler<GetAllNotificationsForUserQuery, GetAllNotificationsForUserQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetAllNotificationsForUserQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetAllNotificationsForUserQueryResponse> Handle(GetAllNotificationsForUserQuery request, CancellationToken cancellationToken)
    {
        GetAllNotificationsForUserQueryResponse response = new();

        GetAllNotificationsForUserQueryValidator validator = new(_repositoryManager);

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

        response.Notifications = await _repositoryManager.NotificationRepository.GetAllNotificationsByUserIdAsync(request.UserId, request.TrackChanges);

        return response;
    }
}