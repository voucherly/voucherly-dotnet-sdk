namespace Voucherly.Sdk.Models;

/// <summary>
/// How a line is sold when the piece is not what it is priced by: a pack of plums is one piece of about 0.75 kg at 1.98 €/kg. Descriptive only: no amount is computed from it, <c>unitAmount</c> times <c>quantity</c> stays what the customer pays, and your till keeps rounding each piece on its own.
/// </summary>
public class PaymentLineUnit : VoucherlyObject
{
    /// <summary>
    /// The unit of measure, such as <c>kg</c> or <c>l</c>. At most 10 characters.
    /// </summary>
    public string? Code { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The nominal content of one piece, in the unit of <c>code</c>. At most three decimals.
    /// </summary>
    public double? Size { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The price per unit of measure, in cents.
    /// </summary>
    public int? Amount { get; set { field = value; MarkAssigned(); } }
}
