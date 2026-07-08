using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.CreatePromoCode;

public class CreatePromoCodeCommandHandler : IRequestHandler<CreatePromoCodeCommand, BaseResponse>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    public CreatePromoCodeCommandHandler(IMapper mapper, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(CreatePromoCodeCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();
        CreatePromoCodeCommandValidator validator = new(_repositoryManager);

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

        PromoCode promoCode = _mapper.Map<PromoCode>(request);

        _repositoryManager.PromoCodeRepository.CreatePromoCode(promoCode);

        await _repositoryManager.SaveAsync();

        return response;
    }
}