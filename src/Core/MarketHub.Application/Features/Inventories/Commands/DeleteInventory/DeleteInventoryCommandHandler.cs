using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Inventories.Commands.DeleteInventory;

public class DeleteInventoryCommandHandler : IRequestHandler<DeleteInventoryCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public DeleteInventoryCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(DeleteInventoryCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        DeleteInventoryCommandValidator validator = new();

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

        Inventory? inventory = await _repositoryManager.InventoryRepository.GetByProductIdAsync(request.ProductId, trackChanges: true);

        if (inventory is null || inventory.Id != request.Id)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Invetory for product with Id: {request.ProductId} is not found.";

            return response;
        }

        _repositoryManager.InventoryRepository.DeleteInventory(inventory);

        await _repositoryManager.SaveAsync();

        return response;
    }
}