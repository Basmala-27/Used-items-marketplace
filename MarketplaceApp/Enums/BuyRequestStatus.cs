namespace MarketplaceApp.Enums
{
    public enum BuyRequestStatus
    {
        Pending,          // في انتظار قبول البائع
        SellerAccepted,   // البائع قبل + 50% تحولت له — بانتظار تأكيد المشتري
        Completed,        // مكتمل — المشتري أكد الاستلام + 50% الثانية للبائع
        RejectedBySeller, // البائع رفض — المبلغ كاملاً رجع للمشتري
        Cancelled         // ألغاه المشتري قبل رد البائع
    }
}
