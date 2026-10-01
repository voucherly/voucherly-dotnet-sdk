namespace Voucherly.Sdk.Enums;

/// <summary>
/// Current status of the Payment.
/// </summary>
public static class PaymentStatus
{
    public const string Requested = "Requested";

    public const string Paid = "Paid";

    public const string Confirmed = "Confirmed";

    public const string Refunded = "Refunded";

    public const string Cancelled = "Cancelled";

    public const string Voided = "Voided";

    public const string Expired = "Expired";
}
