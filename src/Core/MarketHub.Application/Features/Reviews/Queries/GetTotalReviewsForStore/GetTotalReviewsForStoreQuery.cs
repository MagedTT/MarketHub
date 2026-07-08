using MediatR;

namespace MarketHub.Application.Features.Reviews.Queries.GetTotalReviewsForStore;

public class GetTotalReviewsForStoreQuery : IRequest<GetTotalReviewsForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
}