namespace MarketHub.Application.Shared;

public class ProductParameters : RequestParameters
{
    public decimal PriceFrom { get; set; } = 0;
    public decimal PriceTo { get; set; } = int.MaxValue;
    public int RatingFrom { get; set; } = 0;
    public int RatingTo { get; set; } = 5;
    public string Category { get; set; } = string.Empty;
}