export interface StoreProductDto {
    id: string;
    storeId: string;
    brandName: string;
    productName: string;
    productBaseImageUrl: string;
    productPrice: number;
    type: string;
    isActive: boolean;
    numberOfReviews: number;
    averageRating: number;
    numberOfSoldPieces: number;
    amountInStock: number;
}