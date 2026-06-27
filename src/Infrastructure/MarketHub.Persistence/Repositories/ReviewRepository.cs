using System.Security.Cryptography.X509Certificates;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Application.Features.Carts.Queries.GetCartByUserId;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly MarketHubDbContext _context;
    public ReviewRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<Review?> GetReviewByUserIdAsync(Guid userId, bool trackChanges)
    {
        IQueryable<Review> reviews = _context.Reviews;

        if (!trackChanges)
            reviews = reviews.AsNoTracking();

        return await reviews.FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<PagedList<ReviewDto>> GetReviewsForProductWithIdAsync(Guid productId, RequestParameters requestParameters, bool trackChanges)
    {
        IQueryable<Review> reviews = _context.Reviews;

        if (!trackChanges)
            reviews = reviews.AsNoTracking();

        IQueryable<ReviewDto> reviewsDtos = reviews.Where(x => x.ProductId == productId)
            .Select(x => new ReviewDto
            {
                Id = x.Id,
                ReviewerName = x.User.FirstName + " " + x.User.LastName,
                ReviewerRating = x.Rating,
                Comment = x.Comment,
                CreatedAt = x.CreatedAt
            });

        int count = await reviewsDtos.CountAsync();

        List<ReviewDto> reviewsDtosList = await reviewsDtos
            .Skip((requestParameters.PageNumber - 1) * requestParameters.PageSize)
            .Take(requestParameters.PageSize)
            .ToListAsync();

        return new PagedList<ReviewDto>(reviewsDtosList, count, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public void CreateReview(Review review)
        => _context.Reviews.Add(review);

    public void DeleteReview(Review review)
        => _context.Reviews.Remove(review);
}