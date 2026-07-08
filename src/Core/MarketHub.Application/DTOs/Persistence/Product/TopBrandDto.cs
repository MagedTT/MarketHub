namespace MarketHub.Application.DTOs.Persistence.Product;

public class TopBrandDto
{
    public Guid BrandId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalSoldPieces { get; set; }
}