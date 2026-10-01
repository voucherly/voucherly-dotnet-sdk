namespace Voucherly.Sdk.Requests;

public class RefundPaymentRequest : VoucherlyObject
{
    /// <summary>
    /// Indicates whether the refunded amount should be added to the customer's wallet as credit.
    /// </summary>
    public bool? AsCredit { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// List of transactions to be refunded.
    /// </summary>
    public List<RefundPaymentRequestTransaction>? Transactions { get; set { field = value; MarkAssigned(); } }
}
