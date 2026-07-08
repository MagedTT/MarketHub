using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.PromoCodes;
using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetAllPromoCodes;

public class GetAllPromoCodesQueryHandler : IRequestHandler<GetAllPromoCodesQuery, (MetaData metaData, IEnumerable<PromoCodeDto> promoCodes)>
{
    private readonly IRepositoryManager _repositoryManager;
    public GetAllPromoCodesQueryHandler(IRepositoryManager repositoryManager)
        => _repositoryManager = repositoryManager;

    public async Task<(MetaData metaData, IEnumerable<PromoCodeDto> promoCodes)> Handle(GetAllPromoCodesQuery request, CancellationToken cancellationToken)
    {
        PagedList<PromoCodeDto> promoCodesWithMetaData = await _repositoryManager.PromoCodeRepository.GetAllPromoCodesAsync(request.PromoCodeParameters, request.TrackChanges);

        return (metaData: promoCodesWithMetaData.MetaData, promoCodes: promoCodesWithMetaData.AsEnumerable());
    }
}