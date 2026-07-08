using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.GetUsageCountForPromoCodeByCode;

public class GetUsageCountForPromoCodeByCodeQueryHandler : IRequestHandler<GetUsageCountForPromoCodeByCodeQuery, GetUsageCountForPromoCodeByCodeQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetUsageCountForPromoCodeByCodeQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetUsageCountForPromoCodeByCodeQueryResponse> Handle(GetUsageCountForPromoCodeByCodeQuery request, CancellationToken cancellationToken)
    {
        GetUsageCountForPromoCodeByCodeQueryResponse response = new();
        GetUsageCountForPromoCodeByCodeQueryValidator validator = new(_repositoryManager);

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

        response.UsageCount = await _repositoryManager.PromoCodeRepository.GetUsageCountByCodeAsync(request.Code);

        return response;
    }
}