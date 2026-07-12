export interface CreatePromoCode {
    storeId: string;
    code: string;
    discountType: number;
    discountValue: number;
    endDate: Date;
    usageLimit: number;
    isActive: boolean;
}