namespace MarketHub.Application.Shared;

public abstract class BrandParameters : RequestParameters
{
    public string SearchBrandName { get; set; } = string.Empty;
}