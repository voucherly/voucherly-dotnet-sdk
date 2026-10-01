namespace Voucherly.Sdk.Enums;

/// <summary>
/// The type of checkout action required by the payment gateway.
/// </summary>
public static class CheckoutAction
{
    /// <summary>
    /// No interaction is required. The payment is charged server to server.
    /// </summary>
    public const string DIRECT = "DIRECT";

    /// <summary>
    /// One-Time Password authentication is required. The customer needs to enter an OTP code.
    /// </summary>
    public const string OTP = "OTP";

    /// <summary>
    /// A One-Time Password is required together with the italian fiscal code of the payer.
    /// </summary>
    public const string OTP_CF = "OTP_CF";

    /// <summary>
    /// The customer must be redirected to an external URL to complete the payment.
    /// </summary>
    public const string REDIRECT = "REDIRECT";

    /// <summary>
    /// An embedded payment form (drop-in) can be used within your application.
    /// </summary>
    public const string DROPIN = "DROPIN";
}
