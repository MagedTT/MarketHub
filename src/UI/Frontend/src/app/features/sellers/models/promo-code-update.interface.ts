export interface PromoCodeUpate {
    storeId: string;
    promoCodeId: string;
    endDate: Date;
    discountValue: number;
    usageLimit: number;
    isActive: boolean;
    // public Guid StoreId { get; set; }
    // public Guid PromoCodeId { get; set; }
    // public DateTime EndDate { get; set; }
    // public int DiscountValue { get; set; }
    // public int UsageLimit { get; set; }
    // public bool IsActive { get; set; }
}