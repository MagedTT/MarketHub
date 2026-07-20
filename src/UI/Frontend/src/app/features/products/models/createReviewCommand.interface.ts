export interface CreateReviewCommand {
    userId: string;
    productId: string;
    rating: number;
    comment?: string | null;
};