export interface ReviewDto {
    id: string;
    reviewerName: string;
    productId: string;
    reviewerRating: number;
    comment?: string;
    createdAt: Date;
}