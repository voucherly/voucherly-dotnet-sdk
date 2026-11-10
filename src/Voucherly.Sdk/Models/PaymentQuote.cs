namespace Voucherly.Sdk.Models;

/// <summary>
/// What a Payment created from the same body would cost. Nothing is created.
/// </summary>
public class PaymentQuote : VoucherlyObject
{
    /// <summary>
    /// In cents, the sum of the lines before any discount.
    /// </summary>
    public int TotalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// In cents, every discount, on the lines and on the whole Payment.
    /// </summary>
    public int DiscountAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// In cents, what the customer would pay.
    /// </summary>
    public int FinalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// In cents, the part of <c>finalAmount</c> that meal vouchers can pay.
    /// </summary>
    public int FoodAmount { get; set { field = value; MarkAssigned(); } }

    public List<PaymentLine>? Lines { get; set { field = value; MarkAssigned(); } }

    public List<PaymentDiscount>? Discounts { get; set { field = value; MarkAssigned(); } }
}
