export enum OrderStatus {
    Pending = 1,
    Confirmed = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}

export interface OrderParameters {
    pageNumber: number;
    pageSize: number;
    userId?: string;
    orderStatus?: OrderStatus;
    orderByCreationTimeDescending: boolean;
    orderMinTotalPrice: number;
    orderMaxTotalPrice: number;
}