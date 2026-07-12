import { OrderShippingAddressDto } from "./order-shipping-address-dto.interface";

export interface CreateOrderCommand {
    userId: string;
    promoCode?: string | null;
    total: number;
    shippingAddress: OrderShippingAddressDto;
};
