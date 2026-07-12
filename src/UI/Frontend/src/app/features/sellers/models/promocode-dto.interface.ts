export interface PromoCodeDto {
    id: string;
    code: string;
    discountType: number;
    discountValue: number;
    startDate: Date;
    endDate: Date;
    usageLimit: number;
    numberOfTimesUsed: number;
    isActive: boolean;
}