using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Infrastructure;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Products.Commands.AddProductCommand;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, AddProductCommandResponse>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IFileUploadService _fileUploadService;
    public AddProductCommandHandler(IMapper mapper, IRepositoryManager repositoryManager, IFileUploadService fileUploadService)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
        _fileUploadService = fileUploadService;
    }

    public async Task<AddProductCommandResponse> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        AddProductCommandResponse response = new();
        AddProductCommandValidator validator = new(_repositoryManager);

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

        Product product = _mapper.Map<Product>(request);

        Guid productId = await _repositoryManager.ProductRepository.AddProductAsync(product);

        int idx = 1;

        foreach (FileUploadDto file in request.Images)
        {
            (bool status, string message, string filePath) = await _fileUploadService.UploadFile(file);

            ProductImage productImage = new()
            {
                ProductId = productId,
                ImageUrl = filePath,
                DisplayOrder = idx++
            };

            await _repositoryManager.ProductImageRepository.AddImageForProductAsync(productImage);
        }

        Inventory inventory = new()
        {
            ProductId = productId,
            AvailableQuantity = request.AvailableQuantityInStock
        };

        await _repositoryManager.InventoryRepository.AddAmountToProductAsync(inventory);

        await _repositoryManager.SaveAsync();

        response.CreatedProductId = productId;

        return response;
    }
}