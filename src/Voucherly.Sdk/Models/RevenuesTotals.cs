namespace Voucherly.Sdk.Models;

/// <summary>
/// Grand totals of the volumes report, computed across the whole result set.
/// </summary>
public class RevenuesTotals : VoucherlyObject
{
    /// <summary>
    /// Total amount of paid transactions, in cents.
    /// </summary>
    public long PaidAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Amount that was paid but not confirmed (paid minus confirmed), in cents.
    /// </summary>
    public long CancelledAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Total confirmed amount, in cents.
    /// </summary>
    public long ConfirmedAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Total refunded amount, in cents.
    /// </summary>
    public long RefundedAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Net revenue (confirmed minus refunded), in cents.
    /// </summary>
    public long ProfitAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Number of paid transactions.
    /// </summary>
    public int PaidCount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Number of paid-but-not-confirmed transactions.
    /// </summary>
    public int CancelledCount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Number of confirmed transactions.
    /// </summary>
    public int ConfirmedCount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Number of refunded transactions.
    /// </summary>
    public int RefundedCount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Number of transactions contributing to net revenue.
    /// </summary>
    public int ProfitCount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Meal-voucher share of the confirmed amount, in cents. On a partial capture the missing amount is taken from the meal-voucher share last. For the <c>WALLET</c> PaymentGateway it is the customer credit originally funded by meal vouchers.
    /// </summary>
    public long ConfirmedVoucherAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Number of meal vouchers confirmed. Transactions whose PaymentGateway does not report the count are left out and counted in <c>confirmedVoucherCountMissing</c>. Always <c>0</c> for the <c>WALLET</c> PaymentGateway, whose vouchers are counted when the credit is topped up.
    /// </summary>
    public int ConfirmedVoucherCount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Number of transactions with a confirmed meal-voucher share but no voucher count, either because the PaymentGateway did not report it or because only part of the share was captured. When it is not <c>0</c>, <c>confirmedVoucherCount</c> is a lower bound.
    /// </summary>
    public int ConfirmedVoucherCountMissing { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Meal-voucher share refunded to the PaymentGateway, in cents. The refund takes the non-voucher share first. A refund kept as customer credit is not included, since it never reaches the voucher issuer.
    /// </summary>
    public long RefundedVoucherAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Number of meal vouchers refunded. Only a refund of the whole meal-voucher share carries a count; the others are counted in <c>refundedVoucherCountMissing</c>.
    /// </summary>
    public int RefundedVoucherCount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Number of transactions with a refunded meal-voucher share but no voucher count. When it is not <c>0</c>, <c>refundedVoucherCount</c> is a lower bound.
    /// </summary>
    public int RefundedVoucherCountMissing { get; set { field = value; MarkAssigned(); } }
}
