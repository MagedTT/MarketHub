using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommand : IRequest<BaseResponse>
{
    public Guid CurrentUserId { get; set; }
    public Guid ReviewId { get; set; }
}