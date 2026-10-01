using Voucherly.Sdk.Models;

namespace Voucherly.Sdk.Requests;

public class ConfirmPaymentRequestLine : VoucherlyObject
{
    /// <summary>
    /// The quantity delivered, from <c>0</c> to the quantity ordered.
    /// </summary>
    public int Quantity { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Your own reference of the line, as sent at creation. It pairs the line on its own; a value that matches no line of the Payment fails with <c>LINES_MISMATCH</c>, it never falls back to the product.
    /// </summary>
    public string? ExternalId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Product of the line, when it was created with one.
    /// </summary>
    public string? ProductId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The product the line was created with, to pair it by product when the line is sent without its own ids.
    /// </summary>
    public ConfirmPaymentRequestLineProduct? Product { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// One entry per item delivered, only on a line created with <c>product.unit</c>, as many as <c>quantity</c>. Send it when the amount charged differs from the ordered price, as it does with variable-weight items; without it the line is accounted at the ordered price. Fails with <c>PIECES_NOT_ALLOWED</c> on a line without <c>product.unit</c>, and with <c>PIECES_COUNT_MISMATCH</c> when the count differs from <c>quantity</c>.
    /// </summary>
    public List<PaymentLinePiece>? Pieces { get; set { field = value; MarkAssigned(); } }
}
