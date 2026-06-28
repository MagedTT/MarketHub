using System.Net;
using System.Security;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Inventories.Commands.AddStock;

public class AddStockCommandHandler : IRequestHandler<AddStockCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public AddStockCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        AddStockCommandValidator validator = new();

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

        if (inventory is null || inventory.Id != request.InventoryId)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Inventory for product with Id: {request.ProductId} is not found.";

            return response;
        }

        inventory.AvailableQuantity += request.Quantity;

        await _repositoryManager.SaveAsync();

        return response;
    }
}