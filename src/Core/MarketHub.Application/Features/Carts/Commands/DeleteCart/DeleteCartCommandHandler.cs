using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Carts.Commands.DeleteCart;

public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public DeleteCartCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        DeleteCartCommandValidator validator = new(_repositoryManager);

        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count > 0)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ValidationErrors = new();

            foreach (ValidationFailure validationFailure in validationResult.Errors)
                response.ValidationErrors.Add($"{validationFailure.PropertyName},{validationFailure.ErrorMessage}");

            return response;
        }

        Cart? cart = await _repositoryManager.CartRepository.GetCartByIdAsync(request.CartId);

        if (cart is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Cart with Id: {request.CartId} is not found.";

            return response;
        }

        _repositoryManager.CartRepository.DeleteCart(cart);

        await _repositoryManager.SaveAsync();

        return response;
    }
}