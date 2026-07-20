using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Stores.Queries.GetStore;

public class GetStoreQueryResponse : BaseResponse
{
    public StoreDto? Store { get; set; }
    public GetStoreQueryResponse()
        : base()
    { }
}