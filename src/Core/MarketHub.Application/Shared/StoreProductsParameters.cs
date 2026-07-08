namespace MarketHub.Application.Shared;

public class StoreProductsParameters : RequestParameters
{
    public bool OrderByProductPrice { get; set; } = true;
    public bool OrderByNumberOfReviews { get; set; } = false;
    public bool OrderByAverageRating { get; set; } = false;
    public bool OrderByNumberOfSoldPieces { get; set; } = false;
    public bool OrderByAmountInStock { get; set; } = false;
    public bool Descending { get; set; }
}