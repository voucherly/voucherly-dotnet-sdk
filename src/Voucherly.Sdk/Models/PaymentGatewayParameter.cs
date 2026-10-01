namespace Voucherly.Sdk.Models;

/// <summary>
/// A configuration parameter for a payment gateway, including its current value.
/// </summary>
public class PaymentGatewayParameter : VoucherlyObject
{
    /// <summary>
    /// Unique identifier for this parameter.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A human-readable name for this parameter.
    /// </summary>
    public string? FriendlyName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Indicates whether this parameter contains sensitive information that should be masked or hidden.
    /// </summary>
    public bool IsSecret { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The current value of this parameter. May be null if not configured.
    /// </summary>
    public string? Value { get; set { field = value; MarkAssigned(); } }
}
