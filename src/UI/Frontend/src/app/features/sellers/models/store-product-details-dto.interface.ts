export interface StoreProductDetailsDto {
    id: string;
    storeName: string;
    brandName?: string;
    name: string;
    description: string;
    price: number;
    isActive: boolean;
    availableAmountInStock: number;
    type: string;
    specifications: Record<string, any>;
    numberOfReviews: number;
    averageRating: number;
    imagesUrls: string[];
}