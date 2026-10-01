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
}
