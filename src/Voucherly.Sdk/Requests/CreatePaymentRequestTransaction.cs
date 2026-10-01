namespace Voucherly.Sdk.Requests;

/// <summary>
/// A transaction prepared on a specific payment gateway.
/// </summary>
public class CreatePaymentRequestTransaction : VoucherlyObject
{
    /// <summary>
    /// The id of the payment gateway that will process this transaction.
    /// </summary>
    public string PaymentGatewayId { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The amount of this transaction, in cents. When omitted the whole Payment amount is used.
    /// </summary>
    public int? Amount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Honored only by the <c>Wallet</c> and <c>Prepaid</c> payment gateways: every other gateway ignores it. When true and the customer's balance does not cover the amount of this transaction, the transaction is declined instead of drawing what is available. The Payment is created anyway, and the response is a 422 that carries it. When false, the transaction draws as much of the balance as the Payment still needs, and <c>completionMode</c> decides whether the Payment stays open for the rest.
    /// </summary>
    public bool RequireSingleTransactionForWholePayment { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Gateway-specific details of the transaction. Only <c>POS</c> takes them today, to say which card reader must collect the payment: give exactly one of the three identifiers below. The Terminal is looked up across the whole merchant, so it must be paired and, for <c>referenceTerminalId</c>, carry that reference — otherwise the request fails with <c>RESOURCE_MISSING</c> and no Payment is created.
    /// </summary>
    public CreatePaymentRequestTransactionDetails? Details { get; set { field = value; MarkAssigned(); } }
}
