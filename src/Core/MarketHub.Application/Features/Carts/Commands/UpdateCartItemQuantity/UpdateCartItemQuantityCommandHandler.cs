using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Carts.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public UpdateCartItemQuantityCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        UpdateCartItemQuantityCommandValidator validator = new(_repositoryManager);

        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count > 0)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ValidationErrors = new();

            foreach (ValidationFailure failure in validationResult.Errors)
                response.ValidationErrors.Add($"{failure.PropertyName},{failure.ErrorMessage}");

            return response;
        }

        CartItem? cartItem = await _repositoryManager.CartItemRepository.GetCartItemByIdAsync(request.CartItemId);

        if (cartItem is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"CartItem with Id: {request.CartItemId} is not found.";

            return response;
        }

        cartItem.Quantity = request.Quantity;

        _repositoryManager.CartItemRepository.UpdateCartItemQuantity(cartItem);

        await _repositoryManager.SaveAsync();

        return response;
    }
}