namespace Voucherly.Sdk.Models;

/// <summary>
/// Summary information about the Customer's wallet, including total amounts and last update timestamp.
/// </summary>
public class CustomerWalletTotals : VoucherlyObject
{
    /// <summary>
    /// The total amount available in the customer's wallet, in cents.
    /// </summary>
    public long TotalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The total amount of voucher funds available in the customer's wallet, in cents.
    /// </summary>
    public long TotalVoucherAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The total amount of cash funds available in the customer's wallet, in cents.
    /// </summary>
    public long TotalCashAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp of when the wallet totals were last updated.
    /// </summary>
    public DateTimeOffset LastUpdatedOnUtc { get; set { field = value; MarkAssigned(); } }
}
