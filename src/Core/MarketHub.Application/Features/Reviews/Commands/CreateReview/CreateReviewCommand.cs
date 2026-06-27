using MarketHub.Application.Responses;
using MediatR;

namespace MarketHub.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommand : IRequest<BaseResponse>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}