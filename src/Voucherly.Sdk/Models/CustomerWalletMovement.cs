using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// A single movement recorded on the Customer's wallet.
/// </summary>
public class CustomerWalletMovement : VoucherlyObject
{
    /// <summary>
    /// The ID of the Customer this movement belongs to.
    /// </summary>
    public string? CustomerId { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="CustomerWalletAction"/> values.</remarks>
    public string? Action { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Transaction that originated this movement, when applicable.
    /// </summary>
    public string? TransactionId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The total amount of the movement, in cents.
    /// </summary>
    public int Amount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The portion of <c>amount</c> attributable to vouchers, in cents.
    /// </summary>
    public int VoucherAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The portion of <c>amount</c> attributable to cash, in cents.
    /// </summary>
    public int CashAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The wallet total balance after this movement was applied, in cents.
    /// </summary>
    public long? RunningTotalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The wallet voucher balance after this movement was applied, in cents.
    /// </summary>
    public long? RunningTotalVoucherAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The wallet cash balance after this movement was applied, in cents.
    /// </summary>
    public long? RunningTotalCashAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp when this movement was recorded.
    /// </summary>
    public DateTimeOffset OccurredAtUtc { get; set { field = value; MarkAssigned(); } }
}
