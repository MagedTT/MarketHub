using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Infrastructure;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Models.Notification;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Orders.Commands.MarkOrderAsDelivered;

public class MarkOrderAsDeliveredCommandHandler : IRequestHandler<MarkOrderAsDeliveredCommand, BaseResponse>
{
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly IRepositoryManager _repositoryManager;
    public MarkOrderAsDeliveredCommandHandler(IMapper mapper, INotificationService notificationService, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _notificationService = notificationService;
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(MarkOrderAsDeliveredCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();

        MarkOrderAsDeliveredCommandValidator validator = new(_repositoryManager);

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

        Order? order = await _repositoryManager.OrdersRepository.GetOrderByIdAsync(request.OrderId, true);

        if (order is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Order with Id: {request.OrderId} is not found";

            return response;
        }

        if (order.Status != Domain.Enums.OrderStatus.Shipped)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"Can't mark as Delivered";

            return response;
        }

        order.Status = Domain.Enums.OrderStatus.Delivered;

        NotificationDto notificationDto = new()
        {
            UserId = order.UserId,
            ReferenceId = order.Id,
            Title = "Order Delivered",
            Message = $"Order #{order.OrderNumber} has been Delivered",
            Type = Domain.Enums.NotificationType.OrderDelivered
        };

        await _notificationService.SendToUserAsync(order.UserId.ToString(), notificationDto);

        Notification notification = _mapper.Map<Notification>(notificationDto);

        _repositoryManager.NotificationRepository.CreateNotification(notification);

        await _repositoryManager.SaveAsync();

        return response;
    }
}