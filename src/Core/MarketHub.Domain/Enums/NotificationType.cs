namespace MarketHub.Domain.Enums;

public enum NotificationType
{
    OrderCreated = 1,
    OrderConfirmed = 2,
    OrderShipped = 3,
    OrderDelivered = 4,
    OrderCancelled = 5,

    ProductApproved = 6,
    ProductRejected = 7,
    ProductOutOfStock = 8,

    NewReview = 9,

    StoreApproved = 10,
    StoreRejected = 11,

    ReportSubmitted = 12,
    ReportResolved = 13,

    PromoCodeReceived = 14,

    System = 15
}