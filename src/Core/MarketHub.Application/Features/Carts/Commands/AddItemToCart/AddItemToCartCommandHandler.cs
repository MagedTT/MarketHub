using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Carts.Commands.AddItemToCart;

public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public AddItemToCartCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        AddItemToCartCommandValidator validator = new(_repositoryManager);

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

        Guid? cartId = await _repositoryManager.CartRepository.CartExistsByUserIdAsync(request.UserId);

        if (cartId is null)
        {
            Cart cart = new Cart
            {
                UserId = request.UserId
            };

            cartId = await _repositoryManager.CartRepository.CreateCartAsync(cart);
        }

        CartItem cartItem = new CartItem
        {
            CartId = cartId.Value,
            ProductId = request.ProductId,
            Quantity = request.Quantity
        };

        _repositoryManager.CartItemRepository.AddCartItem(cartItem);

        await _repositoryManager.SaveAsync();

        return response;
    }
}