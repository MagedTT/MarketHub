namespace MarketHub.Application.DTOs.Persistence.Review;

public class UpdateReviewRequest
{
    public Guid ReviewId { get; set; }
    public Guid ProductId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}