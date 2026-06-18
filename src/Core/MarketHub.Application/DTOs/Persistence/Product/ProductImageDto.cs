namespace MarketHub.Application.DTOs.Persistence.Product;

public class ProductImageDto
{
    public Guid Id { get; set; }
    public Guid ProductIdId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}