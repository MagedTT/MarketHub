using System.Net;
using FluentValidation.Results;
using MarketHub.Application.Contracts.Persistence;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.GetUsageCountForPromoCode;

public class GetUsageCountForPromoCodeQueryHandler : IRequestHandler<GetUsageCountForPromoCodeQuery, GetUsageCountForPromoCodeQueryResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetUsageCountForPromoCodeQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<GetUsageCountForPromoCodeQueryResponse> Handle(GetUsageCountForPromoCodeQuery request, CancellationToken cancellationToken)
    {
        GetUsageCountForPromoCodeQueryResponse response = new();
        GetUsageCountForPromoCodeQueryValidator validator = new(_repositoryManager);

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

        response.UsageCount = await _repositoryManager.PromoCodeRepository.GetUsageCountByIdAsync(request.PromoCodeId);

        return response;
    }
}