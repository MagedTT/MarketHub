using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IReviewRepository
{
    Task<PagedList<ReviewDto>> GetReviewsForProductWithIdAsync(Guid productId, RequestParameters requestParameters, bool trackChanges);
    Task<ReviewDto?> GetReviewByUserIdAndProductIdAsync(Guid userId, Guid productId, bool trackChanges);
    void CreateReview(Review review);
    void DeleteReview(Review review);
}