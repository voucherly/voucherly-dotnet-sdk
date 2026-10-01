namespace Voucherly.Sdk.Enums;

/// <summary>
/// Why the PaymentGateway declined the Transaction, when it told Voucherly. Only present on a <c>Declined</c> error.
/// </summary>
public static class DeclineErrorCode
{
    public const string Generic = "Generic";

    public const string InsufficientFunds = "InsufficientFunds";

    public const string ExpiredCard = "ExpiredCard";

    public const string CardLimitExceeded = "CardLimitExceeded";

    public const string SuspectedFraud = "SuspectedFraud";

    public const string Fraud = "Fraud";

    public const string StolenOrLostCard = "StolenOrLostCard";

    public const string ClosedAccount = "ClosedAccount";
}
