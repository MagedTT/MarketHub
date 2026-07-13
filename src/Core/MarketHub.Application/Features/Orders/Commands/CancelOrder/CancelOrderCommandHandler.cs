using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Infrastructure;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Exceptions;
using MarketHub.Application.Models.Notification;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, BaseResponse>
{
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly IRepositoryManager _repositoryManager;
    public CancelOrderCommandHandler(IMapper mapper, INotificationService notificationService, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _notificationService = notificationService;
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        CancelOrderCommandValidator validator = new(_repositoryManager);

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

        Order? order = await _repositoryManager.OrdersRepository.GetOrderByIdWithOrderItemsAsync(request.OrderId, trackChanges: true);

        if (order is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Order with Id: {request.OrderId} is not found.";

            return response;
        }

        if (order.UserId != request.UserId)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.Forbidden;
            response.Message = "You are not authorized to cancel this order.";

            return response;
        }

        if (order.Status != Domain.Enums.OrderStatus.Pending)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"Order with Id: {request.OrderId} is already {order.Status.ToString()}.";

            return response;
        }

        order.Status = Domain.Enums.OrderStatus.Cancelled;

        HashSet<Guid> productIds = order.OrderItems.Select(x => x.ProductId).ToHashSet();

        IEnumerable<InventoryReservation> reservations = await _repositoryManager.InventoryReservationRepository.GetReservationsByUserIdAndProductIdsAsync(request.UserId, productIds);

        IEnumerable<Inventory> inventories = await _repositoryManager.InventoryRepository.GetInventoriesByProductIdsAsync(productIds);

        Dictionary<Guid, InventoryReservation> reservationDictionary = reservations.ToDictionary(x => x.ProductId);
        Dictionary<Guid, Inventory> inventoryDictionary = inventories.ToDictionary(x => x.ProductId);

        foreach (OrderItem orderItem in order.OrderItems)
        {
            reservationDictionary.TryGetValue(orderItem.ProductId, out InventoryReservation? inventoryReservation);

            if (inventoryReservation is not null)
                inventoryReservation.Status = Domain.Enums.InventoryReservationStatus.Cancelled;

            inventoryDictionary.TryGetValue(orderItem.ProductId, out Inventory? inventory);

            if (inventory is null)
                throw new InventoryNotFoundException($"Inventory for product with Id: {orderItem.ProductId} is not found.");

            inventory.AvailableQuantity += orderItem.Quantity;
        }

        OrderStatusHistory orderStatusHistory = new()
        {
            OrderId = request.OrderId,
            ChangedByUserId = request.UserId,
            Status = Domain.Enums.OrderStatus.Cancelled
        };

        _repositoryManager.OrderStatusHistoryRepository.Create(orderStatusHistory);

        Guid? storeUserId = await _repositoryManager.StoreRepository.GetStoreUserIdForOrderAsync(order.Id);

        if (storeUserId is null)
            throw new OrderHasNoStoreException($"Order with Id: {order.Id} has no store assigned to it");

        try
        {
            NotificationDto notificationForUserDto = new()
            {
                UserId = order.UserId,
                ReferenceId = order.Id,
                Title = "Order Cancelled",
                Message = $"Order #{order.OrderNumber} has been Cancelled",
                Type = Domain.Enums.NotificationType.OrderCancelled
            };

            NotificationDto notificationForStoreDto = new()
            {
                UserId = storeUserId!.Value,
                ReferenceId = order.Id,
                Title = "Order Cancelled",
                Message = $"Order #{order.OrderNumber} has been Cancelled",
                Type = Domain.Enums.NotificationType.OrderCancelled
            };

            await _notificationService.SendToUserAsync(order.UserId.ToString(), notificationForUserDto);
            await _notificationService.SendToUserAsync(storeUserId?.ToString()!, notificationForStoreDto);


            Notification notificationForUser = _mapper.Map<Notification>(notificationForUserDto);
            Notification notificationForStore = _mapper.Map<Notification>(notificationForStoreDto);

            _repositoryManager.NotificationRepository.CreateNotification(notificationForUser);
            _repositoryManager.NotificationRepository.CreateNotification(notificationForStore);

            await _repositoryManager.SaveAsync();
            return response;
        }
        catch (DbUpdateConcurrencyException)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.Conflict;
            response.Message = "The inventory changed while the order was being cancelled. Please try again.";

            return response;
        }
    }
}