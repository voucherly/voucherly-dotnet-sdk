using Voucherly.Sdk.Enums;
using Voucherly.Sdk.Models;

namespace Voucherly.Sdk.Requests;

/// <summary>
/// Inline description of the product associated with a PaymentLine, used inside the create-payment request body. Contains only product attributes; pricing and discount amounts are line-level fields on <c>Payments.PaymentLine.Request</c>.
/// When the line also specifies a <c>productId</c>, all fields below are optional and any populated value overrides the configuration of the referenced Product. When <c>productId</c> is not specified, <c>name</c> must be populated (and <c>unitAmount</c> must be populated at the line level).
/// </summary>
public class PaymentLineRequestProduct : VoucherlyObject
{
    /// <summary>
    /// An external reference for the product, such as its SKU or EAN, used by Voucherly to reconcile the product across systems and to correctly compute reporting. Highly recommended whenever the product is not referenced via <c>productId</c>. At most 100 characters.
    /// </summary>
    public string? ExternalId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The product’s name, meant to be displayable to the customer. Required when <c>productId</c> is not specified.
    /// </summary>
    public string? Name { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The product’s variant description, meant to be displayable to the customer.
    /// </summary>
    public string? Variant { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The product’s image URL, meant to be displayable to the customer.
    /// </summary>
    public string? Image { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The product's applicable tax rate.
    /// </summary>
    public double? TaxRate { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// What the line is. Defaults to <c>NonFood</c> when omitted, so a grocery item has to declare <c>Food</c> to be payable with meal vouchers.
    /// </summary>
    /// <remarks>One of the <see cref="LineType"/> values.</remarks>
    public string? LineType { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// How the line is sold when the piece is not what it is priced by. Declare it on every line whose price can change at delivery, since only such a line accepts <c>pieces</c> at confirmation.
    /// </summary>
    public PaymentLineUnit? Unit { get; set { field = value; MarkAssigned(); } }
}
