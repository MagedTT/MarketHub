using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IReviewRepository
{
    Task<PagedList<ReviewDto>> GetReviewsForProductWithIdAsync(Guid productId, RequestParameters requestParameters, bool trackChanges);
    Task<Review?> GetReviewByUserIdAsync(Guid userId, bool trackChanges);
    void CreateReview(Review review);
    void DeleteReview(Review review);
}