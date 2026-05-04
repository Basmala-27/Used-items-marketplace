namespace MarketplaceApp.Enums
{
    public enum ItemStatus
    {
        Available,
        Pending,
        Reserved,        // محجوز — في انتظار قبول البائع (Buy Request مرسل)
        PendingDelivery, // قبل البائع — في انتظار تأكيد المشتري للاستلام
        Sold,            // مكتمل — انتقلت الملكية للمشتري
        Swapped,         // تم التبادل بصفقة Swap
    }
}