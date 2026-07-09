import { OrderStatus } from "../../orders/models/order-parameters.interface";

export interface StoreOrdersParameters {
    pageNumber: number;
    pageSize: number;
    OrderStatus?: OrderStatus | null;
};