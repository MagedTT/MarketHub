using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Reviews.Queries.GetTotalReviewsForStore;

public class GetTotalReviewsForStoreQueryResponse : BaseResponse
{
    public int TotalReviews { get; set; }

    public GetTotalReviewsForStoreQueryResponse()
        : base()
    { }
}