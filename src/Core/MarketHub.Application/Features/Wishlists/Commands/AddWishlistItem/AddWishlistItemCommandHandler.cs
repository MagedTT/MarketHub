using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Wishlists.Commands.AddWishlistItem;

public class AddWishlistItemCommandHandler : IRequestHandler<AddWishlistItemCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public AddWishlistItemCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(AddWishlistItemCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        AddWishlistItemCommandValidator validator = new(_repositoryManager);

        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ValidationErrors = new();

            foreach (ValidationFailure failure in validationResult.Errors)
                response.ValidationErrors.Add($"{failure.PropertyName},{failure.ErrorMessage}");

            return response;
        }

        Guid? wishlistId = await _repositoryManager.WishlistRepository.GetWishlistIdByUserIdAsync(request.UserId);

        if (wishlistId is null)
        {
            Wishlist wishlist = new()
            {
                UserId = request.UserId
            };

            wishlistId = wishlist.Id;

            _repositoryManager.WishlistRepository.CreateWishlist(wishlist);
        }

        bool wishlistContainsProduct = await _repositoryManager.WishlistRepository.WishlistContainsProductAsync(wishlistId.Value, request.ProductId);

        if (wishlistContainsProduct)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            response.Message = $"Prouct with Id: {request.ProductId} already exists in the wishlist for user with Id: {request.UserId}";

            return response;
        }

        WishlistItem wishlistItem = new()
        {
            WishlistId = wishlistId.Value,
            ProductId = request.ProductId
        };

        _repositoryManager.WishlistRepository.CreateWishlistItem(wishlistItem);

        await _repositoryManager.SaveAsync();

        return response;
    }
}