namespace Voucherly.Sdk.Models;

/// <summary>
/// Information about the last callback sent to your callbackUrl endpoint.
/// </summary>
public class PaymentLastCallback : VoucherlyObject
{
    /// <summary>
    /// Indicates whether the callback was sent successfully (true) or failed (false).
    /// </summary>
    public bool Success { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp when the last callback was sent.
    /// </summary>
    public DateTimeOffset Date { get; set { field = value; MarkAssigned(); } }
}
