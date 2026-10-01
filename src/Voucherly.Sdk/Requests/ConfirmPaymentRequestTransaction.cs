namespace Voucherly.Sdk.Requests;

public class ConfirmPaymentRequestTransaction : VoucherlyObject
{
    /// <summary>
    /// The id of the transaction.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// If specified, is the partial amount to be confirmed.
    /// </summary>
    public int? Amount { get; set { field = value; MarkAssigned(); } }
}
