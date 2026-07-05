export interface UpdateCartItemQuantityRequest {
    userId: string;
    cartId: string;
    cartItemId: string;
    productId: string;
    quantity: number;
}