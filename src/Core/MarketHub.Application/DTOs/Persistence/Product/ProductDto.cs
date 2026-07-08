namespace MarketHub.Application.DTOs.Persistence.Product;

public class ProductDto
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string ProductBaseImageUrl { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public string Type { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int NumberOfReviews { get; set; }
    public decimal AverageRating { get; set; }
    public int NumberOfSoldPieces { get; set; }
    public int AmountInStock { get; set; }
}