export interface OrderItemDto {
    id: string;
    productId: string;
    quantity: number;
    unitPrice: number;
    lineTotal: number;
    productName: string;
    productBaseImageUrl: string;
    productType: string;
    productSpecifications: Record<string, string>;
}