using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

public class PaymentDiscount : VoucherlyObject
{
    /// <summary>
    /// The discount’s name, meant to be displayable to the customer.
    /// </summary>
    public string? DiscountName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The discount’s description. Use this field to optionally store a long form explanation of the product being sold for your own rendering purposes.
    /// </summary>
    public string? DiscountDescription { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// <c>Line</c> when the discount is already in the <c>discountAmount</c> of the lines it applies to, <c>Subtotal</c> when it comes off the sum of the lines and leaves them as they are.
    /// </summary>
    /// <remarks>One of the <see cref="DiscountLevel"/> values.</remarks>
    public string? Level { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// In cents, what the discount is worth. A discount of <c>level</c> <c>Subtotal</c> comes off the sum of the lines; one of <c>level</c> <c>Line</c> is already in their <c>discountAmount</c> and is not taken off again.
    /// </summary>
    public int Amount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Primary external reference for the discount, used by Voucherly to reconcile it across systems and to correctly compute reporting.
    /// </summary>
    public string? ExternalId1 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Secondary external reference for the discount, used by Voucherly to reconcile it across systems and to correctly compute reporting.
    /// </summary>
    public string? ExternalId2 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The code of the Voucherly coupon that produced this discount, as passed in the <c>couponCode</c> of a discount of the request.
    /// </summary>
    public string? CouponCode { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The code of a promotion priced by the merchant's POS, such as one the customer entered at the table. Voucherly passes it back to the POS and does not validate it.
    /// </summary>
    public string? ExternalCouponCode { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The loyalty points spent to obtain this discount.
    /// </summary>
    public int Points { get; set { field = value; MarkAssigned(); } }
}
