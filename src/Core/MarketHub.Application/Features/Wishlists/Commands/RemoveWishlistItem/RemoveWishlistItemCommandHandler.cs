using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Wishlists.Commands.RemoveWishlistItem;

public class RemoveWishlistItemCommandHandler : IRequestHandler<RemoveWishlistItemCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public RemoveWishlistItemCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(RemoveWishlistItemCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        RemoveWishlistItemCommandValidator validator = new(_repositoryManager);

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

        WishlistItem? wishlistItem = await _repositoryManager.WishlistRepository.GetWishlistItemByProductIdIdAndWishlistIdAsync(request.ProductId, request.WishlistId);

        if (wishlistItem is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"wishlistItem with for user with Id: {request.UserId} is not found";

            return response;
        }

        _repositoryManager.WishlistRepository.DeleteWishlistItem(wishlistItem);

        await _repositoryManager.SaveAsync();

        return response;
    }
}