using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Requests;

public class RefundPaymentRequest : VoucherlyObject
{
    /// <summary>
    /// How every transaction is given back, overriding the <c>refundMode</c> of each one. Defaults to <c>Gateway</c>; <c>NoRefund</c> fails with <c>VALIDATION_ERROR</c>.
    /// </summary>
    /// <remarks>One of the <see cref="RefundMode"/> values.</remarks>
    public string? RefundMode { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// List of transactions to be refunded.
    /// </summary>
    public List<RefundPaymentRequestTransaction>? Transactions { get; set { field = value; MarkAssigned(); } }
}
