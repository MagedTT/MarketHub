using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Products.Commands.DeactivateProductCommand;

public class DeactivateProductCommandHandler : IRequestHandler<DeactivateProductCommand, BaseResponse>
{
    private IRepositoryManager _repositoryManager;
    public DeactivateProductCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        DeactivateProductCommandValidator validator = new(_repositoryManager);

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

        Product product = (await _repositoryManager.ProductRepository.GetByIdAsync(request.ProductId, true))!;

        product.IsActive = false;

        await _repositoryManager.SaveAsync();

        return response;
    }
}