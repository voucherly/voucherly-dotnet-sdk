namespace Voucherly.Sdk.Models;

/// <summary>
/// One item delivered on a line sold by measure, with the amount charged for it.
/// </summary>
public class PaymentLinePiece : VoucherlyObject
{
    /// <summary>
    /// The amount charged for this piece, in cents, net of any discount. It is taken as it is, and it may exceed the ordered price.
    /// </summary>
    public int FinalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The measured content of the piece, in the unit of <c>unit.code</c>. Printed on the documents, never used to compute anything.
    /// </summary>
    public double? Size { get; set { field = value; MarkAssigned(); } }
}
