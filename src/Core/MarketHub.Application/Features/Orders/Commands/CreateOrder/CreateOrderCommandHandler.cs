using System.Data;
using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Infrastructure;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Carts;
using MarketHub.Application.DTOs.Persistence.PromoCodes;
using MarketHub.Application.Exceptions;
using MarketHub.Application.Models.Notification;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MarketHub.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, BaseResponse>
{
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly IRepositoryManager _repositoryManager;
    public CreateOrderCommandHandler(IMapper mapper, INotificationService notificationService, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _notificationService = notificationService;
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        CreateOrderCommandValidator validator = new(_repositoryManager);

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

        ////////////////////////// Need A lookup at the end ////////////////////////////////
        PromoCode? promoCode = null;

        if (request.PromoCode is not null)
        {
            promoCode = await _repositoryManager.PromoCodeRepository.PromoCodeExistsByCodeAsync(request.PromoCode ?? string.Empty);

            if (promoCode is null)
            {
                response.Success = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ValidationErrors = new() { "PromoCode,PromoCode is Invalid." };
                return response;
            }

            else if (
                promoCode.EndDate <= DateTime.Now ||
                promoCode.UsageLimit <= promoCode.NumberOfTimesUsed ||
                !promoCode.IsActive ||
                promoCode.DiscountType == DiscountType.FixedAmount)
            {
                response.Success = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ValidationErrors = new() { "PromoCode,PromoCode is Invalid." };
                return response;
            }
            else
            {
                promoCode.NumberOfTimesUsed++;
            }
        }

        CartDto? cart = await _repositoryManager.CartRepository.GetCartByUserIdAsync(request.UserId);

        if (cart is null || !cart.Items.Any())
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"user with Id: {request.UserId} has not items in cart";

            return response;
        }

        IEnumerable<InventoryReservation> reservations = await _repositoryManager.InventoryReservationRepository.GetActiveReservationsAsync(request.UserId);

        Dictionary<Guid, InventoryReservation> reservationDictionary = reservations.ToDictionary(x => x.ProductId);

        foreach (CartItemDto cartItem in cart.Items)
        {
            bool reservationExists = reservationDictionary.TryGetValue(cartItem.Product.ProductId, out InventoryReservation? reservation);

            if (!reservationExists && reservation is null)
            {
                response.Success = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ValidationErrors = new() { $"Reservation,Reservation for product with Id: {cartItem.Product.ProductId} is not found." };

                return response;
            }

            if (reservation?.ExpiresAt <= DateTime.Now)
            {
                response.Success = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ValidationErrors = new() { $"Reservation,Reservation for product with Id: {cartItem.Product.ProductId} has expired." };

                return response;
            }

            if (reservation?.Quantity != cartItem.Quantity)
            {
                response.Success = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ValidationErrors = new() { $"Reservation,Reserved quantity for product with Id: {cartItem.Product.ProductId} has chanced." };

                return response;
            }
        }

        // decimal totalAmount = cart.Items.Sum(x => x.Quantity * x.Product.ProductPrice);

        // if (promoCode is not null)
        // {
        //     if (promoCode.DiscountType == DiscountType.FixedAmount)
        //     {
        //         if (!(1 <= promoCode.DiscountValue && promoCode.DiscountValue <= totalAmount))
        //         {
        //             response.Success = false;
        //             response.StatusCode = (int)HttpStatusCode.BadRequest;
        //             response.ValidationErrors = new() { $"DiscountValue,Invalid discount value." };

        //             return response;
        //         }

        //         totalAmount -= promoCode.DiscountValue;
        //     }

        //     else if (promoCode.DiscountType == DiscountType.Percentage)
        //     {
        //         if (!(1 <= promoCode.DiscountValue && promoCode.DiscountValue <= 100))
        //         {
        //             response.Success = false;
        //             response.StatusCode = (int)HttpStatusCode.BadRequest;
        //             response.ValidationErrors = new() { $"DiscountValue,Invalid discount value." };

        //             return response;
        //         }

        //         totalAmount = totalAmount - totalAmount * (promoCode.DiscountValue / 100m);
        //     }

        //     promoCode.UsageLimit--;

        //     if (promoCode.UsageLimit < 1)
        //         promoCode.IsActive = false;
        // }



        ShippingAddress shippingAddressToCreate = _mapper.Map<ShippingAddress>(request.ShippingAddress);

        Order order = new()
        {
            UserId = request.UserId,
            Status = OrderStatus.Pending,
            ShippingAddress = shippingAddressToCreate,
            PromoCodeId = promoCode?.Id,
            // TotalAmount = totalAmount
            TotalAmount = request.Total
        };

        Guid? storeUserId = await _repositoryManager.StoreRepository.GetStoreUserIdForProductAsync(cart.Items.First().Product.ProductId);

        if (storeUserId is null)
            throw new OrderHasNoStoreException("Products don't belong to any store");

        foreach (CartItemDto cartItem in cart.Items)
        {
            order.OrderItems.Add(new OrderItem
            {
                OrderId = order.Id,
                ProductId = cartItem.Product.ProductId,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.Product.ProductPrice
            });
        }

        order.OrderStatusHistories.Add(new OrderStatusHistory
        {
            OrderId = order.Id,
            ChangedByUserId = request.UserId,
            Status = OrderStatus.Pending
        });

        foreach (InventoryReservation inventoryReservation in reservations)
        {
            inventoryReservation.Inventory.ReservedQuantity -= inventoryReservation.Quantity;
            inventoryReservation.Status = InventoryReservationStatus.Completed;

            if (inventoryReservation.Inventory.AvailableQuantity == 0)
            {
                NotificationDto notificationDto = new()
                {
                    UserId = storeUserId.Value,
                    ReferenceId = inventoryReservation.ProductId,
                    Title = "Product Went out of stock",
                    Message = $"A Product went out of stock click to check it",
                    Type = NotificationType.ProductOutOfStock
                };

                await _notificationService.SendToUserAsync(storeUserId.ToString()!, notificationDto);

                Notification notification = _mapper.Map<Notification>(notificationDto);

                _repositoryManager.NotificationRepository.CreateNotification(notification);
            }
        }

        Cart? cartToDelete = await _repositoryManager.CartRepository.GetCartByIdAsync(cart.CartId);

        _repositoryManager.CartRepository.DeleteCart(cartToDelete ?? new());

        _repositoryManager.OrdersRepository.CreateOrder(order);

        try
        {
            NotificationDto notificationDto = new()
            {
                UserId = storeUserId.Value,
                ReferenceId = order.Id,
                Title = "Order Created",
                Message = $"Order #{order.OrderNumber} has been Created",
                Type = NotificationType.OrderCreated
            };

            await _notificationService.SendToUserAsync(storeUserId.ToString()!, notificationDto);

            Notification notification = _mapper.Map<Notification>(notificationDto);

            _repositoryManager.NotificationRepository.CreateNotification(notification);

            await _repositoryManager.SaveAsync();
            return response;
        }

        catch (DbUpdateConcurrencyException)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.Conflict;
            response.Message = "The order could not be created because one or more resources were modified by another operation. Please refresh the data and try again.";

            return response;
        }
    }
}