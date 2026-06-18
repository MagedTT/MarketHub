namespace MarketHub.Application.DTOs.Persistence.Product;

public class ProductCardDto
{
    public Guid Id { get; set; }
    public string? BrandName { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int AvailableAmountInStock { get; set; }
    public string Type { get; set; } = string.Empty;
    public int NumberOfReviews { get; set; }
    public int AverageRating { get; set; }
    public byte[] BaseImage { get; set; } = default!;
}