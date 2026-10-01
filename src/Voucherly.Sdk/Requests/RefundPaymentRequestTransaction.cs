namespace Voucherly.Sdk.Requests;

public class RefundPaymentRequestTransaction : VoucherlyObject
{
    /// <summary>
    /// The id of the transaction.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// If specified, is the partial amount to be refunded.
    /// </summary>
    public int? Amount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Indicates whether the refunded amount should be added to the customer's wallet as credit.
    /// </summary>
    public bool? AsCredit { get; set { field = value; MarkAssigned(); } }
}
