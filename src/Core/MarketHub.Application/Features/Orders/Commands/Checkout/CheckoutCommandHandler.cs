using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Carts;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MarketHub.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Application.Features.Orders.Commands.Checkout;

public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public CheckoutCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        CheckoutCommandValidator validator = new(_repositoryManager);

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

        CartDto? cart = await _repositoryManager.CartRepository.GetCartByUserIdAsync(request.UserId);

        if (cart is null || !cart.Items.Any())
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Cart for user with Id: {request.UserId} is not found.";

            return response;
        }

        HashSet<Guid> productsIds = cart.Items.Select(x => x.Product.ProductId).ToHashSet();

        IEnumerable<Inventory> inventories = await _repositoryManager.InventoryRepository.GetInventoriesByProductIdsAsync(productsIds);
        IEnumerable<InventoryReservation> reservations = await _repositoryManager.InventoryReservationRepository.GetReservationsAsync(request.UserId);

        Dictionary<Guid, Inventory> inventoryDictionary = inventories.ToDictionary(x => x.ProductId);
        Dictionary<Guid, InventoryReservation> reservationDictionary = reservations.ToDictionary(x => x.ProductId);
        DateTime now = DateTime.Now;

        foreach (InventoryReservation inventoryReservation in reservations)
        {
            if (!productsIds.Contains(inventoryReservation.ProductId))
            {
                if (inventoryReservation.Status == InventoryReservationStatus.Active && inventoryReservation.ExpiresAt > now)
                {
                    inventoryReservation.Inventory.AvailableQuantity += inventoryReservation.Quantity;
                    inventoryReservation.Inventory.ReservedQuantity -= inventoryReservation.Quantity;
                }

                inventoryReservation.Status = InventoryReservationStatus.Cancelled;
            }
        }

        foreach (CartItemDto cartItem in cart.Items)
        {
            if (!inventoryDictionary.TryGetValue(cartItem.Product.ProductId, out Inventory? inventory))
            {
                response.Success = false;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Message =
                    $"Inventory for product with Id: {cartItem.Product.ProductId} was not found.";

                return response;
            }

            bool reservationExists = reservationDictionary.TryGetValue(cartItem.Product.ProductId, out InventoryReservation? reservation);

            if (reservationExists && reservation is not null)
            {
                if (reservation.Status == InventoryReservationStatus.Active)
                {
                    inventory.AvailableQuantity += reservation.Quantity;
                    inventory.ReservedQuantity -= reservation.Quantity;
                }

                reservation.Status = InventoryReservationStatus.Active;
                reservation.Quantity = cartItem.Quantity;
                reservation.ReservedAt = now;
                reservation.ExpiresAt = now.AddMinutes(5);
            }

            else
            {
                reservation = new()
                {
                    UserId = request.UserId,
                    ProductId = cartItem.Product.ProductId,
                    InventoryId = inventory.Id,
                    Quantity = cartItem.Quantity,
                    Status = InventoryReservationStatus.Active,
                    ReservedAt = now,
                    ExpiresAt = now.AddMinutes(5)
                };

                _repositoryManager.InventoryReservationRepository.CreateInventoryReservation(reservation);
            }

            if (inventory.AvailableQuantity < cartItem.Quantity)
            {
                response.Success = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ValidationErrors = new() { $"{cartItem.Product.ProductName},{cartItem.Product.ProductName} does not have enough stock." };

                return response;
            }

            inventory.AvailableQuantity -= cartItem.Quantity;
            inventory.ReservedQuantity += cartItem.Quantity;
        }

        try
        {
            await _repositoryManager.SaveAsync();

            return response;
        }
        catch (DbUpdateConcurrencyException)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.Conflict;
            response.Message =
                "Inventory changed while you were checking out. Please try again.";

            return response;
        }
    }
}