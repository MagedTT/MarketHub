using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.Carts.Queries.GetAmountInCart;

public class GetAmountInCartQueryHandler : IRequestHandler<GetAmountInCartQuery, GetAmountInCartQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetAmountInCartQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetAmountInCartQueryResponse> Handle(GetAmountInCartQuery request, CancellationToken cancellationToken)
    {
        GetAmountInCartQueryResponse response = new();
        GetAmountInCartQueryValidator validator = new(_repositoryManager);

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

        response.Amount = await _repositoryManager.CartRepository.GetAmountInCartByUserIdAsync(request.UserId);

        return response;
    }
}