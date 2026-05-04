namespace MarketplaceApp.Enums
{
    public enum OrderStatus
    {
        Pending,              // في انتظار قبول البائع
        PartiallyPaid,        // البائع قبل وتحولت له 50%
        Completed,            // مكتمل بعد تأكيد الاستلام
        Cancelled,            // ملغى بواسطة المشتري
        RejectedBySeller      // مرفوض من البائع
    }
}