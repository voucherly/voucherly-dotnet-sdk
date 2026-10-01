namespace Voucherly.Sdk.Models;

/// <summary>
/// Aggregated spending history of the Customer.
/// </summary>
public class SpendData : VoucherlyObject
{
    /// <summary>
    /// The total amount the Customer has spent, in cents.
    /// </summary>
    public long TotalSpentAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The total amount refunded to the Customer, in cents.
    /// </summary>
    public long TotalRefundedAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The number of Payments the Customer has completed.
    /// </summary>
    public int NoOfPayments { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The average amount of the Customer's Payments, in cents.
    /// </summary>
    public int AveragePaymentAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The date and time of the Customer's first Payment, in UTC. The property name carries a typo that is part of the wire format.
    /// </summary>
    public DateTimeOffset FirstPaymentOnOtc { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The date and time of the Customer's most recent Payment, in UTC.
    /// </summary>
    public DateTimeOffset LastPaymentOnUtc { get; set { field = value; MarkAssigned(); } }
}
