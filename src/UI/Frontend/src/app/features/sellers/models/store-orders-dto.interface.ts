import { OrderStatus } from "../../orders/models/order-parameters.interface";
import { StoreOrderItemDto } from "./store-order-item-dto.interface";
import { StoreOrderShippingAddressDto } from "./store-order-shipping-address-dto.interface";

export interface StoreOrderDto {
    orderId: string;
    storeId: string;
    userId: string;
    userName: string;
    promoCode: string | null;
    orderNumber: string;
    status: OrderStatus;
    totalAmount: number;
    createdAt: Date;
    shippingAddress: StoreOrderShippingAddressDto;
    orderItems: StoreOrderItemDto[];
}