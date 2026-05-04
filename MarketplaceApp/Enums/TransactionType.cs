namespace MarketplaceApp.Enums
{
    public enum TransactionType
    {
        Purchase,       // شراء منتج (الحجز الأولي — 100% في Escrow)
        PartialPayment, // دفعة جزئية (50% عند قبول البائع)
        FinalPayment,   // الدفعة الأخيرة (50% عند تأكيد الاستلام)
        Deposit,        // شحن رصيد (TopUp)
        Refund,         // استرداد (رفض البائع أو إلغاء)
        SwapRecord      // تسجيل صفقة تبادل
    }
}
