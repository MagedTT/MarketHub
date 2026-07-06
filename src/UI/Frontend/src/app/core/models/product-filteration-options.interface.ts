export interface ProductFilterationOptions {
    priceFrom: number;
    priceTo: number;
    ratingFrom: number;
    ratingTo: number;
    categories: {
        electronics: false,
        phones: false,
        tvs: false
    };
}