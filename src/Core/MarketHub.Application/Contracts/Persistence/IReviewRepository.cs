using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IReviewRepository
{
    Task<Review?> GetReviewByIdAsync(Guid reviewId);
    Task<PagedList<ReviewDto>> GetReviewsForProductWithIdAsync(Guid productId, RequestParameters requestParameters, bool trackChanges);
    Task<ReviewDto?> GetReviewDtoByUserIdAndProductIdAsync(Guid userId, Guid productId, bool trackChanges);
    Task<Review?> GetReviewByUserIdAndProductIdAsync(Guid userId, Guid productId);
    Task<bool> ReviewExists(Guid userId, Guid productId);
    void CreateReview(Review review);
    void DeleteReview(Review review);
}