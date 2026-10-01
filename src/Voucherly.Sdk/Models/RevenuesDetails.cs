namespace Voucherly.Sdk.Models;

/// <summary>
/// A volumes report row, aggregating turnover for a single PaymentGateway (and, when grouped by Store and/or Company, a single Store and/or Company).
/// </summary>
public class RevenuesDetails : VoucherlyObject
{
    /// <summary>
    /// Unique identifier of the PaymentGateway this row aggregates.
    /// </summary>
    public string? PaymentGatewayId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Display name of the PaymentGateway.
    /// </summary>
    public string? PaymentGatewayName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Unique identifier of the Store this row aggregates. Only populated when <c>groupBy</c> includes <c>Store</c>.
    /// </summary>
    public string? StoreId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Display name of the Store. Only populated when <c>groupBy</c> includes <c>Store</c>.
    /// </summary>
    public string? StoreName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The first external identifier of the Store, as you set it. Only populated when <c>groupBy</c> includes <c>Store</c>.
    /// </summary>
    public string? StoreExternalId1 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The second external identifier of the Store, as you set it. Only populated when <c>groupBy</c> includes <c>Store</c>.
    /// </summary>
    public string? StoreExternalId2 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Unique identifier of the Company this row aggregates. Only populated when <c>groupBy</c> includes <c>Company</c>.
    /// </summary>
    public string? CompanyId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Display name of the Company. Only populated when <c>groupBy</c> includes <c>Company</c>.
    /// </summary>
    public string? CompanyName { get; set { field = value; MarkAssigned(); } }

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
    /// Breakdown of this row by secondary (child) PaymentGateway, when applicable.
    /// </summary>
    public List<ChildRevenuesDetails>? ChildPaymentGateways { get; set { field = value; MarkAssigned(); } }
}
