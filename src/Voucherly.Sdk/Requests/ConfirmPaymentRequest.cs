using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Requests;

/// <summary>
/// Specify at most one of:
/// - nothing, to capture every authorized transaction in full;
/// - <c>transactions</c>, to choose yourself which transactions to capture and for how much;
/// - <c>lines</c>, to confirm the quantities actually delivered: Voucherly adds them up, discounts of the Payment included, and settles the transactions on that amount;
/// - <c>finalAmount</c> with <c>foodAmount</c>, to confirm at an amount without resending the lines.
/// A confirmation accounts, it does not rewrite the order: the lines, the discounts and the amounts of the Payment keep describing what was ordered, and what was settled is in <c>confirmedAmount</c>, <c>cancelledAmount</c> and <c>refundedAmount</c>, and per line in <c>confirmedQuantity</c>.
/// With <c>lines</c> or with the amounts, meal vouchers cover at most the food amount and the other transactions cover the rest: authorizations are captured partially or voided. Money already captured is given back only as <c>refundMode</c> allows.
/// </summary>
public class ConfirmPaymentRequest : VoucherlyObject
{
    /// <summary>
    /// List of transactions to be confirmed.
    /// </summary>
    public List<ConfirmPaymentRequestTransaction>? Transactions { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Every line of the Payment with the quantity delivered, <c>0</c> for a line not delivered. Each one is paired with a line of the Payment by your own <c>externalId</c> first and, when it is not sent, by product: <c>productId</c>, then <c>product.externalId</c>, then product name and variant, in any order; lines sharing the same product are paired in order of appearance, which is why sending your own <c>externalId</c> is the safe choice.
    /// Prices stay the ones ordered, a discount on the whole line follows the quantity, and the discounts of the Payment are applied to the amount delivered. The one way a line can cost more than ordered is <c>pieces</c>, on a line created with <c>product.unit</c>: their amounts replace the ordered price, within what the <c>PreauthorizationMargin</c> line authorised. The line keeps what was ordered and carries the result in <c>confirmedQuantity</c>, <c>confirmedFinalAmount</c> and <c>confirmedPieces</c>. A line missing or matching no line of the Payment fails with <c>LINES_MISMATCH</c>; a quantity above the ordered one fails with <c>QUANTITY_EXCEEDS_ORDERED</c>. The <c>PreauthorizationMargin</c> line is the exception: leave it out, or send it with quantity <c>0</c>.
    /// </summary>
    public List<ConfirmPaymentRequestLine>? Lines { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount to confirm, in cents. It only drives what is captured, voided and given back on the transactions. Requires <c>foodAmount</c>.
    /// </summary>
    public int? FinalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The part of <c>finalAmount</c> that meal vouchers can pay, in cents. Requires <c>finalAmount</c>, and can't be higher than it.
    /// </summary>
    public int? FoodAmount { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="RefundMode"/> values.</remarks>
    public string? RefundMode { get; set { field = value; MarkAssigned(); } }
}
