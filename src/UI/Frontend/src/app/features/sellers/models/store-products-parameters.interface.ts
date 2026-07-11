export interface StoreProductsParameters {
    pageNumber: number;
    pageSize: number;
    orderByProductPrice: boolean;
    orderByNumberOfReviews: boolean;
    orderByAverageRating: boolean;
    orderByNumberOfSoldPieces: boolean;
    orderByAmountInStock: boolean;
    descending: boolean;
}