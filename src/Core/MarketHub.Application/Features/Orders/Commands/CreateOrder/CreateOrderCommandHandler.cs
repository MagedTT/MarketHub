using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Carts;
using MarketHub.Application.DTOs.Persistence.PromoCodes;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MarketHub.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public CreateOrderCommandHandler(IRepositoryManager repositoryManager)
    {
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
                response.Message = $"Reservation for product with Id: {cartItem.Product.ProductId} is not found.";
                return response;
            }

            if (reservation?.ExpiresAt <= DateTime.Now)
            {
                response.Success = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = $"Reservation for product with Id: {cartItem.Product.ProductId} has expired.";
                return response;
            }

            if (reservation?.Quantity != cartItem.Quantity)
            {
                response.Success = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = $"Reserved quantity for product with Id: {cartItem.Product.ProductId} has chanced.";
                return response;
            }
        }

        decimal totalAmount = cart.Items.Sum(x => x.Quantity * x.Product.ProductPrice);

        if (promoCode is not null)
        {
            if (promoCode.DiscountType == DiscountType.FixedAmount)
            {
                if (!(1 <= promoCode.DiscountValue && promoCode.DiscountValue <= totalAmount))
                {
                    response.Success = false;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = $"Invalid discount value.";

                    return response;
                }

                totalAmount -= promoCode.DiscountValue;
            }

            else if (promoCode.DiscountType == DiscountType.Percentage)
            {
                if (!(1 <= promoCode.DiscountValue && promoCode.DiscountValue <= 100))
                {
                    response.Success = false;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return response;
                }

                totalAmount = totalAmount - totalAmount * (promoCode.DiscountValue / 100m);
            }

            promoCode.UsageLimit--;

            if (promoCode.UsageLimit < 1)
                promoCode.IsActive = false;
        }

        Order order = new()
        {
            UserId = request.UserId,
            Status = OrderStatus.Pending,
            ShippingAddress = request.ShippingAddress,
            PromoCodeId = promoCode?.Id,
            TotalAmount = totalAmount
        };

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
        }

        Cart? cartToDelete = await _repositoryManager.CartRepository.GetCartByIdAsync(cart.CartId);

        _repositoryManager.CartRepository.DeleteCart(cartToDelete ?? new());

        _repositoryManager.OrdersRepository.CreateOrder(order);

        try
        {
            await _repositoryManager.SaveAsync();
            return response;
        }
        catch (DbUpdateConcurrencyException)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.Conflict;

            return response;
        }
    }
}