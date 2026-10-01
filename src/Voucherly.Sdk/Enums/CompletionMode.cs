namespace Voucherly.Sdk.Enums;

/// <summary>
/// When the Payment counts as paid. Defaults to <c>Standard</c>. With <c>Standard</c> the Payment stays open until its whole amount is paid, so the customer can combine several payment gateways, such as meal vouchers and a card for the rest. With <c>Partial</c> the first successful transaction completes the Payment, whatever it paid: <c>amount</c> is set to the paid amount, and anything left is settled outside this Payment. A failed transaction leaves the Payment open for another attempt. With <c>AnyTransaction</c> the Payment gets a single attempt: it completes as with <c>Partial</c> when the first transaction succeeds, and is voided when it fails. Use it on a card reader, where a declined card has to end the Payment. With <c>Partial</c> and <c>AnyTransaction</c> the checkout does not capture the transactions it authorized, so confirm the Payment or set <c>isAutoConfirm</c>. Both require a subscription plan that includes partial payments, except in sandbox.
/// </summary>
public static class CompletionMode
{
    public const string Standard = "Standard";

    public const string Partial = "Partial";

    public const string AnyTransaction = "AnyTransaction";
}
