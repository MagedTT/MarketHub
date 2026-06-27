using MediatR;

namespace MarketHub.Application.Features.Reviews.Queries.GetReviewByUserId;

public class GetReviewByUserIdAndProductIdQuery : IRequest<GetReviewByUserIdAndProductIdQueryResponse>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public bool TrackChanges { get; set; }
}