using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Carts.Commands.RemoveCartItem;

public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public RemoveCartItemCommandHandler(IRepositoryManager repositoryManager)
       => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        RemoveCartItemCommandValidator validator = new(_repositoryManager);

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

        CartItem? cartItem = await _repositoryManager.CartItemRepository.GetCartItemByIdAsync(request.CartItemId);

        if (cartItem is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"CartItem with Id: {request.CartItemId} is not found.";
            return response;
        }

        _repositoryManager.CartItemRepository.DeleteCartItem(cartItem);

        await _repositoryManager.SaveAsync();

        return response;
    }
}