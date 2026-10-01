namespace Voucherly.Sdk.Requests;

/// <summary>
/// Request shape of a PaymentLine, used in the create-payment request body. Product attributes are grouped under a nested <c>product</c> object; pricing and discount amounts stay at the line level.
/// Each line must specify the product through one of:
/// - <c>productId</c> only — all product details are loaded from the referenced Voucherly Product.
/// - <c>product</c> only — inline ad-hoc product description; required fields inside <c>product</c> must be populated.
/// - <c>productId</c> + <c>product</c> — the line is linked to the referenced Product, and any field set inside <c>product</c> overrides the corresponding value of the Product configuration (useful to force a specific price, name, etc.).
/// </summary>
public class PaymentLineRequest : VoucherlyObject
{
    /// <summary>
    /// The quantity of the line item being purchased.
    /// </summary>
    public int Quantity { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A non-negative integer in cents representing how much to charge for each individual unit. Required when <c>productId</c> is not specified.
    /// </summary>
    public int? UnitAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A non-negative integer in cents representing the discount applied to each individual unit. This field is mutually exclusive with <c>discountAmount</c> — only one of the two may be specified.
    /// </summary>
    public int UnitDiscountAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A non-negative integer in cents representing the total discount applied to the entire line. This field is mutually exclusive with <c>unitDiscountAmount</c> — only one of the two may be specified.
    /// </summary>
    public int DiscountAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of an existing Product in Voucherly that this PaymentLine refers to. One of <c>productId</c> or <c>product</c> is required.
    /// </summary>
    public string? ProductId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Your own reference of the line, such as the id of the order line in your system, when you have one. It is a reference of the line, not of its product, and at confirmation it pairs the line on its own, which is what tells two lines of the same product apart. At most 100 characters.
    /// </summary>
    public string? ExternalId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Inline description of the product associated with a PaymentLine, used inside the create-payment request body. Contains only product attributes; pricing and discount amounts are line-level fields on <c>Payments.PaymentLine.Request</c>.
    /// When the line also specifies a <c>productId</c>, all fields below are optional and any populated value overrides the configuration of the referenced Product. When <c>productId</c> is not specified, <c>name</c> must be populated (and <c>unitAmount</c> must be populated at the line level).
    /// </summary>
    public PaymentLineRequestProduct? Product { get; set { field = value; MarkAssigned(); } }

    public List<PaymentLineModifier>? Modifiers { get; set { field = value; MarkAssigned(); } }
}
