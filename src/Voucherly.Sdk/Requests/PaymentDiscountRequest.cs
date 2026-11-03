namespace Voucherly.Sdk.Requests;

public class PaymentDiscountRequest : VoucherlyObject
{
    /// <summary>
    /// The discount’s name, meant to be displayable to the customer. Not allowed with <c>couponCode</c>.
    /// </summary>
    public string DiscountName { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The discount’s description. Use this field to optionally store a long form explanation of the discount for your own rendering purposes. Not allowed with <c>couponCode</c>.
    /// </summary>
    public string? DiscountDescription { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A positive integer in cents taken off the subtotal, the sum of the lines after their own discounts. Voucherly spreads it on every line except gifts, in proportion to their amount, to split it by VAT rate on the receipt and between food and non-food. Required unless the discount carries a <c>couponCode</c>.
    /// </summary>
    public int? Amount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The code of a coupon created in Voucherly, as the customer typed it: case and spaces do not count. A discount with a <c>couponCode</c> takes no other field: Voucherly names the discount after the coupon and prices it by its rules. A coupon on products, a gift or the delivery becomes a discount of <c>level</c> <c>Line</c>, taken off the <c>discountAmount</c> of the lines it applies to; a coupon on the whole order becomes a discount of <c>level</c> <c>Subtotal</c>. Up to 5 discounts of a Payment can carry a code. A code that does not apply stops the Payment with a 400 whose <c>parameter</c> is <c>Discounts</c>, whose <c>couponCode</c> is the code and whose <c>code</c> says why: <c>COUPON_NOT_FOUND</c>, <c>COUPON_NOT_ACTIVE</c>, <c>COUPON_NOT_YET_VALID</c>, <c>COUPON_EXPIRED</c>, <c>COUPON_MIN_ORDER</c> (with <c>minOrderAmount</c>), <c>COUPON_NOT_APPLICABLE</c>, <c>COUPON_EXHAUSTED</c>, <c>COUPON_ALREADY_USED</c>, <c>COUPON_REQUIRES_CUSTOMER</c>, <c>COUPON_NOT_COMBINABLE</c>, <c>COUPON_NOT_AVAILABLE_AT_TABLE</c>, <c>COUPON_FIRST_ORDER_ONLY</c>.
    /// </summary>
    public string? CouponCode { get; set { field = value; MarkAssigned(); } }
}
