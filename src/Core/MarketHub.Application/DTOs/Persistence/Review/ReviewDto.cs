namespace MarketHub.Application.DTOs.Persistence.Review;

public class ReviewDto
{
    public Guid Id { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    // public Guid ProductId { get; set; }
    public int ReviewerRating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}