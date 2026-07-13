using MarketHub.Application.Features.Notifications.Commands.MarkAllNotificationAsReadForUser;
using MarketHub.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using MarketHub.Application.Features.Notifications.Queries.GetAllNotificationsForUser;
using MarketHub.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MarketHub.API.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;
    public NotificationsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    [Route("all/{userId:guid}")]
    public async Task<IActionResult> GetAllNotifications(Guid userId)
    {
        GetAllNotificationsForUserQuery request = new() { UserId = userId, TrackChanges = false };

        GetAllNotificationsForUserQueryResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return Ok(response.Notifications);
    }

    [HttpPost]
    [Route("markAllAsRead/{userId:guid}")]
    public async Task<IActionResult> MarkAllNotificationsAsRead(Guid userId)
    {
        MarkAllNotificationAsReadForUserCommand request = new() { UserId = userId };

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return NoContent();
    }

    [HttpPost]
    [Route("markNotificationAsRead/{userId:guid}/{notificationId:guid}")]
    public async Task<IActionResult> markNotificationAsRead(Guid userId, Guid notificationId)
    {
        MarkNotificationAsReadCommand request = new() { UserId = userId, NotificationId = notificationId };

        BaseResponse response = await _mediator.Send(request);

        if (!response.Success && response.StatusCode == StatusCodes.Status400BadRequest)
        {
            foreach (string error in response.ValidationErrors!)
            {
                string[] errorDetails = error.Split(',');
                ModelState.TryAddModelError(errorDetails[0], errorDetails[1]);
            }

            return BadRequest(ModelState);
        }

        return NoContent();
    }
}