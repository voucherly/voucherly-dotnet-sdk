namespace Voucherly.Sdk.Enums;

/// <summary>
/// The type of error returned.
/// </summary>
public static class TransactionErrorCode
{
    /// <summary>
    /// The PaymentGateway refused the Transaction without a code Voucherly maps.
    /// </summary>
    public const string Generic = "Generic";

    /// <summary>
    /// The payer abandoned the Transaction.
    /// </summary>
    public const string Cancelled = "Cancelled";

    /// <summary>
    /// The Transaction needs a PaymentMethod and none was supplied.
    /// </summary>
    public const string MissingPaymentMethod = "MissingPaymentMethod";

    /// <summary>
    /// The PaymentGateway declined the Transaction; <c>declineErrorCode</c> says why, when it says.
    /// </summary>
    public const string Declined = "Declined";

    /// <summary>
    /// The PaymentGateway accepted the Transaction but left it pending its own review.
    /// </summary>
    public const string Uncleared = "Uncleared";

    /// <summary>
    /// Strong customer authentication did not complete.
    /// </summary>
    public const string AuthenticationFailed = "AuthenticationFailed";

    /// <summary>
    /// The requested amount is above what the meal vouchers presented can cover.
    /// </summary>
    public const string ExceedVoucherAmount = "ExceedVoucherAmount";

    /// <summary>
    /// The card present Terminal did not answer.
    /// </summary>
    public const string TerminalOffline = "TerminalOffline";

    /// <summary>
    /// No Terminal matches the one the Transaction asked for.
    /// </summary>
    public const string TerminalNotFound = "TerminalNotFound";

    /// <summary>
    /// The Terminal is already running another Transaction.
    /// </summary>
    public const string TerminalBusy = "TerminalBusy";

    /// <summary>
    /// The PaymentGateway account of the merchant is not configured for this Transaction.
    /// </summary>
    public const string MerchantConfiguration = "MerchantConfiguration";

    /// <summary>
    /// Voucherly failed to complete the Transaction.
    /// </summary>
    public const string System = "System";
}
