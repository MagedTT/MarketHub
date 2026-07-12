export interface PromoCodeParameters {
    pageNumber: number;
    pageSize: number;
    storeId?: string;
    discountType?: number;
    startDate?: Date;
    endDate?: Date;
    isActive?: boolean;
    numberOfTimesUsed?: number;
    usageLimitMin: number;
    usageLimitMax: number;
    orderByDiscountValue: boolean;
    orderByStartDate: boolean;
    orderByEndDate: boolean;
    orderByUsageLimit: boolean;
    orderByNumberOfTimesUsed: boolean;
    descending: boolean;
}