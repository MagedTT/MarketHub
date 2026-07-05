import { CartProductDto } from "./cart-product-dto.interface";

export interface CartItemDto {
    cartItemId: string;
    quantity: number;
    subTotal: number;
    product: CartProductDto;
}