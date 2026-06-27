using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Queries.GetReviewByUserId;

public class GetReviewByUserIdAndProductIdQueryResponse : BaseResponse
{
    public ReviewDto? Review { get; set; }
    public GetReviewByUserIdAndProductIdQueryResponse()
        : base()
    { }
}