using MediatR;

namespace MarketHub.Application.Features.Reviews.Queries.GetRatingCountForStore;

public class GetRatingCountForStoreQuery : IRequest<GetRatingCountForStoreQueryResponse>
{
    public Guid StoreId { get; set; }
}