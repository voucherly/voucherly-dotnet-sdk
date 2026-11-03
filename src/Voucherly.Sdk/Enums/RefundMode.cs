namespace Voucherly.Sdk.Enums;

/// <summary>
/// How money already captured is given back, on a confirmation at lower amounts and on a refund. Authorizations are never refunded: they are captured partially or voided.
/// - <c>NoRefund</c>: nothing is given back. The default of a confirmation, which then fails with <c>REFUND_REQUIRED</c> without touching any transaction; not accepted by a refund.
/// - <c>Gateway</c>: refunded through the payment gateway, the default of a refund. When its configuration forbids a partial refund, the request fails before touching any transaction; when the gateway itself refuses the refund it has already been asked for, the operations already done stay done and <c>operations</c> lists them.
/// - <c>Credit</c>: credited to the customer's wallet, without calling the gateway. A credit coming from meal vouchers can be spent only on food. Not accepted on a wallet top-up.
/// - <c>GatewayOrCredit</c>: refunded through the gateway where it allows it, credited to the wallet otherwise, also when the gateway refuses the refund. Not accepted on a wallet top-up.
/// </summary>
public static class RefundMode
{
    public const string NoRefund = "NoRefund";

    public const string Gateway = "Gateway";

    public const string Credit = "Credit";

    public const string GatewayOrCredit = "GatewayOrCredit";
}
