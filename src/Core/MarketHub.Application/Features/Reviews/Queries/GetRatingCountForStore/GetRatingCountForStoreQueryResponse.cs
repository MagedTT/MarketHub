using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Reviews.Queries.GetRatingCountForStore;

public class GetRatingCountForStoreQueryResponse : BaseResponse
{
    public IEnumerable<StoreRatingCount> StoreRatingCount { get; set; } = new List<StoreRatingCount>();

    public GetRatingCountForStoreQueryResponse()
        : base()
    { }
}