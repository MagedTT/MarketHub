import { CartItemDto } from "./cart-item-dto.interface";

export interface CartDto {
    cartId: string;
    createdAt: Date;
    cartItemsDto: CartItemDto[];
}