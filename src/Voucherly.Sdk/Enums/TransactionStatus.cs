namespace Voucherly.Sdk.Enums;

/// <summary>
/// Current status of the Transaction.
/// </summary>
public static class TransactionStatus
{
    public const string RequestFailed = "RequestFailed";

    public const string Requested = "Requested";

    public const string Paid = "Paid";

    public const string Confirmed = "Confirmed";

    public const string Refunded = "Refunded";

    public const string RefundedPartially = "RefundedPartially";

    public const string Cancelled = "Cancelled";

    public const string Failed = "Failed";

    public const string Voided = "Voided";

    public const string Expired = "Expired";

    public const string Reversed = "Reversed";

    public const string ImpossibleRefund = "ImpossibleRefund";

    public const string NextAction = "NextAction";
}
