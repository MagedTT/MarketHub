import { OrderItemDto } from "./order-item-dto.interface";
import { OrderStatus } from "./order-parameters.interface";

export interface OrderDto {
    id: string;
    orderNumber: string;
    orderedByUserName: string;
    numberOfOrderedProducts: number;
    status: OrderStatus;
    createdAt: Date;
    dateOfDelivery: Date;
    shippingAddress: string;
    totalAmount: number;
    promoCode?: string;
    items: OrderItemDto[];
}