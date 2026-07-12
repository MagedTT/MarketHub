export interface PromoCodeEditModel {
    id: string;
    code: string;
    endDate: Date;
    discountValue: number;
    usageLimit: number;
    numberOfTimesUsed: number;
    isActive: boolean;
}