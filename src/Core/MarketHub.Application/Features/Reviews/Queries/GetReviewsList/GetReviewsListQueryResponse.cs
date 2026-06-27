using System.Reflection.Metadata.Ecma335;
using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Application.Responses;
using MarketHub.Application.Shared;

namespace MarketHub.Application.Features.Reviews.Queries.GetReviewsList;

public class GetReviewsListQueryResponse : BaseResponse
{
    public IEnumerable<ReviewDto>? Reviews { get; set; }
    public MetaData? MetaData { get; set; }

    public GetReviewsListQueryResponse()
        : base()
    { }
}