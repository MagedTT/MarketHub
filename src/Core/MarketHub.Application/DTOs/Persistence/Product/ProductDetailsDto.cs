using System.Text.Json;
using MarketHub.Application.DTOs.Persistence.Review;

namespace MarketHub.Application.DTOs.Persistence.Product;

public class ProductDetailsDto
{
    public Guid Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string? BrandName { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int AvailableAmountInStock { get; set; }
    public string Type { get; set; } = string.Empty;
    public JsonElement Specifications { get; set; }
    public int NumberOfReviews { get; set; }
    public int AverageRating { get; set; }
    public byte[] BaseImage { get; set; } = default!;
    public ICollection<ReviewDto>? Reviews { get; set; }
    public ICollection<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
}