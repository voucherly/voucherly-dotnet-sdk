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
    /// A non-negative integer in cents representing how much to subtract from the total. Must be specified when both <c>type</c> and <c>value</c> are null.
    /// </summary>
    public int Amount { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="DiscountType"/> values.</remarks>
    public string? Type { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The value of the discount, contextual to the selected <c>type</c>. Must be specified when <c>type</c> is provided and <c>amount</c> is null.
    /// </summary>
    public int? Value { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Defines the application order of this discount relative to others.
    /// </summary>
    public int? Index { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Primary external reference for the discount, used by Voucherly to reconcile it across systems and to correctly compute reporting.
    /// </summary>
    public string? ExternalId1 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Secondary external reference for the discount, used by Voucherly to reconcile it across systems and to correctly compute reporting.
    /// </summary>
    public string? ExternalId2 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The coupon code the payer entered to obtain this discount.
    /// </summary>
    public string? CouponCode { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The loyalty points spent to obtain this discount.
    /// </summary>
    public int Points { get; set { field = value; MarkAssigned(); } }
}
