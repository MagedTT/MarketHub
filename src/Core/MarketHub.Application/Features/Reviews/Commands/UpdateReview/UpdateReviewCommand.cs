using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Commands.UpdateReview;

public class UpdateReviewCommand : IRequest<BaseResponse>
{
    public Guid ReviewId { get; set; }
    public Guid CurrentUserId { get; set; }
    public Guid ProductId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}