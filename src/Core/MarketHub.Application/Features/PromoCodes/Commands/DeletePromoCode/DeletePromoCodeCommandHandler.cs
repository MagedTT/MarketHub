using System.Net;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Commands.DeletePromoCode;

public class DeletePromoCodeCommandHandler : IRequestHandler<DeletePromoCodeCommand, BaseResponse>
{
    private readonly IRepositoryManager _repositoryManager;
    public DeletePromoCodeCommandHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<BaseResponse> Handle(DeletePromoCodeCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();

        PromoCode? promoCode = await _repositoryManager.PromoCodeRepository.GetByIdAsync(request.PromoCodeId);

        if (promoCode is null)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"PromoCode with Id: {request.PromoCodeId} is not found.";

            return response;
        }

        if (promoCode.NumberOfTimesUsed > 0)
        {
            response.Success = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"PromoCode with Id: {request.PromoCodeId} has been used.";

            return response;
        }

        _repositoryManager.PromoCodeRepository.DeletePromoCode(promoCode);
        await _repositoryManager.SaveAsync();

        return response;
    }
}