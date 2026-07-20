export interface ReviewDto {
    id: string;
    reviewerName: string;
    productId: string;
    reviewerId: string;
    reviewerRating: number;
    comment?: string;
    createdAt: Date;
}