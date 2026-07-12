using System.Net;
using AutoMapper;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.UpdatePromoCode;

public class UpdatePromoCodeCommandHandler : IRequestHandler<UpdatePromoCodeCommand, BaseResponse>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    public UpdatePromoCodeCommandHandler(IMapper mapper, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(UpdatePromoCodeCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();

        UpdatePromoCodeCommandValidator validator = new(_repositoryManager);

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

        PromoCode? promoCode = await _repositoryManager.PromoCodeRepository.GetByIdAsync(request.PromoCodeId);

        if (promoCode is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"PromoCode with Id: {request.PromoCodeId} is not found.";

            return response;
        }

        _mapper.Map(request, promoCode);

        await _repositoryManager.SaveAsync();

        return response;
    }
}