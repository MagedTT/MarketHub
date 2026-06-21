using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Carts.Commands.CreateCart;

public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, CreateCartCommandResponse>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    public CreateCartCommandHandler(IMapper mapper, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    public async Task<CreateCartCommandResponse> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        CreateCartCommandResponse response = new();
        CreateCartCommandValidator validator = new(_repositoryManager);

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

        Cart cart = _mapper.Map<Cart>(request);

        response.CartId = await _repositoryManager.CartRepository.CreateCartAsync(cart);

        await _repositoryManager.SaveAsync();

        return response;
    }
}