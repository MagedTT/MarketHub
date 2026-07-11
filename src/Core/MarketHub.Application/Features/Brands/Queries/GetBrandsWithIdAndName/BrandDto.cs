namespace MarketHub.Application.Features.Brands.Queries.GetBrandsWithIdAndName;

public class BrandDto
{
    public Guid BrandId { get; set; }
    public string Name { get; set; } = string.Empty;
}