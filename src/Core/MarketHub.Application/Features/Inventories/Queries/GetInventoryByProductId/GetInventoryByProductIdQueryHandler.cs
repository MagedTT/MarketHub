using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Inventories;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Inventories.Queries.GetInventoryByProductId;

public class GetInventoryByProductIdQueryHandler : IRequestHandler<GetInventoryByProductIdQuery, GetInventoryByProductIdQueryResponse>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    public GetInventoryByProductIdQueryHandler(IMapper mapper, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    public async Task<GetInventoryByProductIdQueryResponse> Handle(GetInventoryByProductIdQuery request, CancellationToken cancellationToken)
    {
        GetInventoryByProductIdQueryResponse response = new();
        GetInventoryByProductIdQueryValidator validator = new();

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

        InventoryDto? inventoryDto = await _repositoryManager.InventoryRepository.GetInventoryDtoByProductIdAsync(request.ProductId, request.TrackChanges);

        if (inventoryDto is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Inventory for product with Id: {request.ProductId} is not found.";
            return response;
        }

        response.Inventory = inventoryDto;

        return response;
    }
}