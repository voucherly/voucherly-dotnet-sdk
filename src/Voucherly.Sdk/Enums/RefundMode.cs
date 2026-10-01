namespace Voucherly.Sdk.Enums;

/// <summary>
/// How money already captured is given back when the confirmed amounts are lower than what was captured. Authorizations are never refunded: they are captured partially or voided.
/// - <c>NoRefund</c> (default): nothing is given back, and the request fails with <c>REFUND_REQUIRED</c> without touching any transaction.
/// - <c>Gateway</c>: refunded through the payment gateway. When its configuration forbids a partial refund, the request fails before touching any transaction; when the gateway itself refuses the refund it has already been asked for, the captures of this confirmation are done and <c>operations</c> lists them.
/// - <c>Credit</c>: credited to the customer's wallet, without calling the gateway. A credit coming from meal vouchers can be spent only on food.
/// - <c>GatewayOrCredit</c>: refunded through the gateway where it allows it, credited to the wallet otherwise.
/// </summary>
public static class RefundMode
{
    public const string NoRefund = "NoRefund";

    public const string Gateway = "Gateway";

    public const string Credit = "Credit";

    public const string GatewayOrCredit = "GatewayOrCredit";
}
