import { ReviewDto } from "./review-dto.interface";

export interface ProductDetailsDto {
    id: string;
    storeName: string;
    brandName?: string;
    name: string;
    description: string;
    price: number;
    availableAmountInStock: number;
    type: string;
    specifications: Record<string, any>;
    numberOfReviews: number;
    averageRating: number;
    reviews?: ReviewDto[];
    imagesUrls: string[];
}