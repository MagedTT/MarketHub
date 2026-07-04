export interface ProductCardModel {
    id: string;
    brandName?: string;
    name: string;
    description: string;
    price: number;
    availableAmountInStock: number;
    type: string;
    numberOfReviews: number;
    averageRating: number;
    baseImageUrl: string;
}