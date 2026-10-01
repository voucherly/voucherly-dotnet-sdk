namespace Voucherly.Sdk.Models;

/// <summary>
/// Information about when and whether the checkout was closed by the customer.
/// </summary>
public class PaymentCloseCheckout : VoucherlyObject
{
    /// <summary>
    /// Indicates whether the checkout was closed successfully (true) or cancelled (false).
    /// </summary>
    public bool Success { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp when the checkout was closed.
    /// </summary>
    public DateTimeOffset Date { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Why the checkout could not be closed, as a dotted code such as <c>Payment.Confirm</c> or <c>TableOrder.AlreadyClosed</c>. Only present when <c>success</c> is false.
    /// </summary>
    public string? ErrorReason { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The payload the failing step attached to <c>errorReason</c>, as a JSON string. Its shape depends on the reason.
    /// </summary>
    public string? ErrorAdditionalData { get; set { field = value; MarkAssigned(); } }
}
