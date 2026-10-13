using Voucherly.Sdk.Enums;

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
    /// How this transaction is given back, when the request sets no <c>refundMode</c> of its own. Defaults to <c>Gateway</c>; <c>NoRefund</c> fails with <c>VALIDATION_ERROR</c>.
    /// </summary>
    /// <remarks>One of the <see cref="RefundMode"/> values.</remarks>
    public string? RefundMode { get; set { field = value; MarkAssigned(); } }
}
