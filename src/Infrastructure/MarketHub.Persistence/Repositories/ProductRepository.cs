using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
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

    public async Task<Product?> GetByIdAsync(Guid id, bool trackChanges)
    {
        IQueryable<Product> products = _context.Products;

        if (!trackChanges)
            products = products.AsNoTracking();

        return await products.FirstOrDefaultAsync(x => x.Id == id);
    }

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
                x.IsActive,
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
            IsActive = product.IsActive,
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

    public async Task<int> TotalProductsByStoreIdAsync(Guid storeId)
        => await _context.Products.CountAsync(x => x.StoreId == storeId);

    public async Task<int> TotalProductsInStockByStoreIdAsync(Guid storeId)
        => await _context.Products.CountAsync(x => x.StoreId == storeId && x.Inventory.AvailableQuantity > 0);

    public async Task<int> TotalProductsOutOfStockByStoreIdAsync(Guid storeId)
        => await _context.Products.CountAsync(x => x.StoreId == storeId && x.Inventory.AvailableQuantity < 1);

    public async Task<PagedList<ProductDto>> GetAllProductsByStoreIdAsync(Guid storeId, int? productStatus, StoreProductsParameters storeProductsParameters)
    {
        IQueryable<Product> products = _context.Products.Where(x => x.StoreId == storeId);

        if (productStatus == 1)
            products = products.Where(x => x.IsActive);
        else if (productStatus == 2)
            products = products.Where(x => !x.IsActive);

        if (storeProductsParameters.Descending)
        {
            if (storeProductsParameters.OrderByAmountInStock)
                products = products.OrderByDescending(x => x.Inventory.AvailableQuantity);
            else if (storeProductsParameters.OrderByAverageRating)
                products = products.OrderByDescending(x => x.AverageRating);
            else if (storeProductsParameters.OrderByNumberOfReviews)
                products = products.OrderByDescending(x => x.NumberOfReviews);
            else if (storeProductsParameters.OrderByNumberOfSoldPieces)
                products = products.OrderByDescending(x => x.NumberOfSoldPieces);
            else if (storeProductsParameters.OrderByProductPrice)
                products = products.OrderByDescending(x => x.Price);
        }
        else
        {
            if (storeProductsParameters.OrderByAmountInStock)
                products = products.OrderBy(x => x.Inventory.AvailableQuantity);
            else if (storeProductsParameters.OrderByAverageRating)
                products = products.OrderBy(x => x.AverageRating);
            else if (storeProductsParameters.OrderByNumberOfReviews)
                products = products.OrderBy(x => x.NumberOfReviews);
            else if (storeProductsParameters.OrderByNumberOfSoldPieces)
                products = products.OrderBy(x => x.NumberOfSoldPieces);
            else if (storeProductsParameters.OrderByProductPrice)
                products = products.OrderBy(x => x.Price);
        }

        int count = await products.CountAsync();

        List<ProductDto> productDtos = await products.Select(x => new ProductDto
        {
            Id = x.Id,
            StoreId = x.StoreId,
            BrandName = x.Brand != null ? x.Brand.Name : string.Empty,
            ProductName = x.Name,
            ProductBaseImageUrl = x.Images.Select(x => x.ImageUrl).FirstOrDefault() ?? string.Empty,
            ProductPrice = x.Price,
            Type = x.Type,
            IsActive = x.IsActive,
            NumberOfReviews = x.NumberOfReviews,
            AverageRating = x.AverageRating,
            NumberOfSoldPieces = x.NumberOfSoldPieces,
            AmountInStock = x.Inventory.AvailableQuantity
        })
        .Skip((storeProductsParameters.PageNumber - 1) * storeProductsParameters.PageSize)
        .Take(storeProductsParameters.PageSize)
        .ToListAsync();

        return new PagedList<ProductDto>(productDtos, count, storeProductsParameters.PageNumber, storeProductsParameters.PageSize);
    }

    public async Task<IEnumerable<ProductDto>> TopNBestSellingProductsByStoreIdAsync(Guid storeId, int n)
    {
        return await _context.Products
            .Where(x => x.StoreId == storeId)
            .OrderByDescending(x => x.NumberOfSoldPieces)
            .Take(n)
            .Select(x => new ProductDto
            {
                Id = x.Id,
                StoreId = x.StoreId,
                BrandName = x.Brand != null ? x.Brand.Name : string.Empty,
                ProductName = x.Name,
                ProductBaseImageUrl = x.Images.Select(x => x.ImageUrl).FirstOrDefault() ?? string.Empty,
                ProductPrice = x.Price,
                Type = x.Type,
                IsActive = x.IsActive,
                NumberOfReviews = x.NumberOfReviews,
                AverageRating = x.AverageRating,
                NumberOfSoldPieces = x.NumberOfSoldPieces,
                AmountInStock = x.Inventory.AvailableQuantity
            }).ToListAsync();
    }

    public async Task<bool> StoreOwnsProductAsync(Guid storeId, Guid productId)
        => await _context.Products.AnyAsync(x => x.Id == productId && x.StoreId == storeId);

    public async Task<StoreProductDetailsDto?> GetProductDetailsByStoreIdAsync(Guid storeId, Guid productId)
    {
        return await _context.Products
        .Where(x => x.Id == productId && x.StoreId == storeId)
        .Select(x => new StoreProductDetailsDto
        {
            Id = x.Id,
            StoreName = x.Store.Name,
            BrandName = x.Brand != null ? x.Brand.Name : string.Empty,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            IsActive = x.IsActive,
            AvailableAmountInStock = x.Inventory.AvailableQuantity,
            Type = x.Type,
            Specifications = JsonSerializer.Deserialize<JsonElement>(x.Specifications),
            NumberOfReviews = x.NumberOfReviews,
            AverageRating = x.AverageRating,
            ImagesUrls = x.Images.Select(x => x.ImageUrl).ToList()
        }).FirstOrDefaultAsync();
    }

    public async Task<bool> CheckProductExistsForStoreByIsAsync(Guid productId)
        => await _context.Products.AnyAsync(x => x.Id == productId);
}