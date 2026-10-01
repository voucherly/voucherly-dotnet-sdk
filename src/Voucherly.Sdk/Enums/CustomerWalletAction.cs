namespace Voucherly.Sdk.Enums;

/// <summary>
/// Type of wallet movement:
/// - <c>PAYMENT</c>: amount debited from the wallet to settle a Payment.
/// - <c>PAYMENT_R</c>: reversal of a <c>PAYMENT</c> movement.
/// - <c>WALLET</c>: amount credited to the wallet (e.g., refund issued as credit).
/// - <c>WALLET_R</c>: reversal of a <c>WALLET</c> movement.
/// - <c>ADJUSTMENT</c>: manual adjustment performed by an operator.
/// - <c>EXCESS</c>: amount credited to the wallet because a Transaction collected more than the Payment required.
/// - <c>EXCESS_R</c>: reversal of an <c>EXCESS</c> movement.
/// </summary>
public static class CustomerWalletAction
{
    public const string PAYMENT = "PAYMENT";

    public const string PAYMENT_R = "PAYMENT_R";

    public const string WALLET = "WALLET";

    public const string WALLET_R = "WALLET_R";

    public const string ADJUSTMENT = "ADJUSTMENT";

    public const string EXCESS = "EXCESS";

    public const string EXCESS_R = "EXCESS_R";
}
