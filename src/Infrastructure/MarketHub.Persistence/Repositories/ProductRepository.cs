using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.DTOs.Persistence.Review;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MarketHubDbContext _context;
    public ProductRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<ProductCardDto?> GetProductCardByIdAsync(Guid id, bool trackChanges)
    {
        ProductCardDto? productCard = await _context.Products.Where(product => product.Id == id)
            .Select(x => new ProductCardDto
            {
                Id = x.Id,
                BrandName = x.Brand != null ? x.Brand.Name : null,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                AvailableAmountInStock = x.Inventory.AvailableQuantity,
                Type = x.Type,
                // NumberOfReviews = x.Reviews.Count,
                NumberOfReviews = x.NumberOfReviews,
                AverageRating = x.AverageRating,
                BaseImageUrl = x.Images.Select(x => x.ImageUrl).FirstOrDefault() ?? string.Empty
            }).FirstOrDefaultAsync();

        return productCard;
    }

    public async Task<PagedList<ProductCardDto>> GetProductCardsAsync(ProductParameters productParameters, bool trackChanges)
    {
        IQueryable<Product> products = _context.Products;

        if (!trackChanges)
            products = products.AsNoTracking();

        products = products.Where(x =>
            !x.IsDeleted &&
            x.IsActive &&
            productParameters.PriceFrom <= x.Price &&
            x.Price <= productParameters.PriceTo &&
            productParameters.RatingFrom <= x.AverageRating &&
            x.AverageRating <= productParameters.RatingTo);

        if (!string.IsNullOrEmpty(productParameters.Category))
            products = products.Where(x => x.Type == productParameters.Category);

        IQueryable<ProductCardDto> productCards = products
            .Select(x => new ProductCardDto
            {
                Id = x.Id,
                BrandName = x.Brand != null ? x.Brand.Name : null,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                AvailableAmountInStock = x.Inventory.AvailableQuantity,
                Type = x.Type,
                NumberOfReviews = x.NumberOfReviews,
                AverageRating = x.AverageRating,
                BaseImageUrl = x.Images.Select(x => x.ImageUrl).FirstOrDefault() ?? string.Empty
            });

        int count = await productCards.CountAsync();

        List<ProductCardDto> productCardsList = await productCards.Skip((productParameters.PageNumber - 1) * productParameters.PageSize).Take(productParameters.PageSize).ToListAsync();

        return new PagedList<ProductCardDto>(productCardsList, count, productParameters.PageNumber, productParameters.PageSize);
    }

    public async Task<ProductDetailsDto?> GetProductDetailsByIdAsync(Guid id, bool trackChanges)
    {
        IQueryable<Product> products = _context.Products.Where(x => x.Id == id && !x.IsDeleted && x.IsActive);

        if (!trackChanges)
            products = products.AsNoTracking();

        var product = await products
            .Select(x => new
            {
                x.Id,
                StoreName = x.Store.Name,
                BrandName = x.Brand != null ? x.Brand.Name : null,
                x.Name,
                x.Description,
                x.Price,
                AvailableAmountInStock = x.Inventory.AvailableQuantity,
                x.Type,
                x.Specifications,
                x.NumberOfReviews,
                x.AverageRating,
                Reviews = x.Reviews.Take(20).Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ReviewerName = r.User.FirstName + " " + r.User.LastName,
                    ReviewerRating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                }).ToList(),
                ImagesUrls = x.Images.Select(i => i.ImageUrl).ToList()
            }).FirstOrDefaultAsync();

        if (product is null)
            return null;

        ProductDetailsDto productDetails = new ProductDetailsDto
        {
            Id = product.Id,
            StoreName = product.StoreName,
            BrandName = product.BrandName,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            AvailableAmountInStock = product.AvailableAmountInStock,
            Type = product.Type,
            Specifications = JsonSerializer.Deserialize<JsonElement>(product.Specifications),
            NumberOfReviews = product.NumberOfReviews,
            AverageRating = product.AverageRating,
            Reviews = product.Reviews,
            ImagesUrls = product.ImagesUrls
        };

        return productDetails;
    }

    public async Task<bool> CheckProductExistsByIsAsync(Guid productId)
        => await _context.Products.AnyAsync(x => x.Id == productId && !x.IsDeleted && x.IsActive);

    public Task<Guid> AddProductAsync(Product product)
    {
        _context.Products.Add(product);

        return Task.FromResult(product.Id);
    }

    public void UpdateProductAsync(Product product)
    {
        // _context.Entry(product).State = EntityState.Modified;

        // return Task.CompletedTask;

        _context.Products.Update(product);
    }

    public async Task MarkAsDeletedAsync(Guid id)
    {
        Product? product = await _context.Products.FindAsync(id);

        if (product is null)
            return;

        product.IsDeleted = true;
        product.IsActive = false;
    }
}