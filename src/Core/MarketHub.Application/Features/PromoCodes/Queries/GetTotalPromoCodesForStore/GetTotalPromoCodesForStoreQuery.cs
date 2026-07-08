using MediatR;

namespace MarketHub.Application.Features.PromoCodes.Queries.GetTotalPromoCodesForStore;

public class GetTotalPromoCodesForStoreQuery : IRequest<GetTotalPromoCodesForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
}