// public decimal PriceFrom { get; set; } = 0;
// public decimal PriceTo { get; set; } = 10_000;
// public int RatingFrom { get; set; } = 0;
// public int RatingTo { get; set; } = 5;
// public string Category { get; set; } = string.Empty;

export interface ProductParameters {
    pageNumber: number;
    pageSize: number;
    priceFrom: number;
    priceTo: number;
    ratingFrom: number;
    ratingTo: number;
    category: string;
}