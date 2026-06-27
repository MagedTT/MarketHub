using MarketHub.Application.Shared;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Queries.GetReviewsList;

public class GetReviewsListQuery : IRequest<GetReviewsListQueryResponse>
{
    public Guid ProductId { get; set; }
    public RequestParameters RequestParameters { get; set; } = new();
    public bool TrackChanges { get; set; }
}