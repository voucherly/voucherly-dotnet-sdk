using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// Response shape of a PaymentLine, returned within a Payment object.
/// Note: when creating a Payment, the request body uses a different, normalized shape that nests product fields under a <c>product</c> object. See <c>Payments.PaymentLine.Request</c> for the create-payment request line shape.
/// </summary>
public class PaymentLine : VoucherlyObject
{
    /// <summary>
    /// The quantity of the line item being purchased.
    /// </summary>
    public int Quantity { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A non-negative integer in cents representing how much to charge for each individual unit.
    /// </summary>
    public int UnitAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A non-negative integer in cents representing the discount applied to each individual unit.
    /// </summary>
    public int UnitDiscountAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A non-negative integer in cents representing the total discount applied to the entire line.
    /// </summary>
    public int DiscountAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// <c>unitAmount</c> times <c>quantity</c>, in cents. Compute it from those two instead.
    /// </summary>
    [Obsolete("unitAmount times quantity, in cents. Compute it from those two instead.")]
    public int TotalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// <c>unitDiscountAmount</c> times <c>quantity</c>, in cents. Compute it from those two instead.
    /// </summary>
    [Obsolete("unitDiscountAmount times quantity, in cents. Compute it from those two instead.")]
    public int TotalDiscountAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A non-negative integer in cents representing the final amount charged for this line, after discounts.
    /// </summary>
    public int FinalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Product that this PaymentLine refers to, when the line was created against an existing Voucherly Product.
    /// </summary>
    public string? ProductId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The product’s name, meant to be displayable to the customer.
    /// </summary>
    public string? ProductName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The product’s variant description, meant to be displayable to the customer.
    /// </summary>
    public string? ProductVariant { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The product’s image URL, meant to be displayable to the customer.
    /// </summary>
    public string? ProductImage { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The external reference of the product of the line, as sent in <c>product.externalId</c>: the SKU or code you use for it. Distinct from the reference of the line.
    /// </summary>
    public string? ProductExternalId1 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Your own reference of the line, as sent at creation, typically the id of the order line in your system. At confirmation it pairs the line on its own.
    /// </summary>
    public string? ExternalId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// How the line is sold when the piece is not what it is priced by, as sent at creation. Absent on a line sold by the piece.
    /// </summary>
    public PaymentLineUnit? Unit { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The product's applicable tax rate.
    /// </summary>
    public double? TaxRate { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="LineType"/> values.</remarks>
    public string? LineType { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// True when <c>lineType</c> is <c>Food</c>. Read <c>lineType</c> instead.
    /// </summary>
    [Obsolete("True when lineType is Food. Read lineType instead.")]
    public bool IsFood { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Indicates whether this PaymentLine is a gift item.
    /// </summary>
    public bool IsGift { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="LineOrigin"/> values.</remarks>
    public string? Origin { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The quantity the confirmation accounted for, when it differs from the one ordered. Absent while the line stands as ordered, which is what every other field describes.
    /// </summary>
    public int? ConfirmedQuantity { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The final amount of the line as confirmed, in cents, discount of the line included. Absent together with <c>confirmedQuantity</c>.
    /// </summary>
    public int? ConfirmedFinalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The pieces the line was confirmed with, one per item delivered, when the confirmation sent them. Their amounts add up to <c>confirmedFinalAmount</c>, which may then exceed <c>finalAmount</c>.
    /// </summary>
    public List<PaymentLinePiece>? ConfirmedPieces { get; set { field = value; MarkAssigned(); } }
}
