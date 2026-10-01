namespace Voucherly.Sdk.Models;

/// <summary>
/// Error information returned by an external payment gateway provider.
/// </summary>
public class ExternalError : VoucherlyObject
{
    /// <summary>
    /// A human-readable error message from the payment gateway.
    /// </summary>
    public string? Message { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// An error code from the payment gateway provider.
    /// </summary>
    public string? Code { get; set { field = value; MarkAssigned(); } }
}
