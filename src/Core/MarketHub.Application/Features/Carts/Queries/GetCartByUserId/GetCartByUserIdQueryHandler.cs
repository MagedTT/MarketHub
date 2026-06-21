using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Carts;
using MediatR;

namespace MarketHub.Application.Features.Carts.Queries.GetCartByUserId;

public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, GetCartByUserIdQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetCartByUserIdQueryHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<GetCartByUserIdQueryResponse> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
    {
        GetCartByUserIdQueryResponse response = new();
        GetCartByUserIdQueryValidator validator = new(_repositoryManager);

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

        CartDto? cartDto = await _repositoryManager.CartRepository.GetCartByUserIdAsync(request.UserId);

        if (cartDto is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Cart for user with id: {request.UserId} is not found.";

            return response;
        }

        response.Cart = cartDto;

        return response;
    }
}