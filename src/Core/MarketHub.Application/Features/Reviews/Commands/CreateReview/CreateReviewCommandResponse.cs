using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Application.Responses;

namespace MarketHub.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandResponse : BaseResponse
{
    public ReviewDto? Review { get; set; }

    public CreateReviewCommandResponse()
        : base()
    { }
}