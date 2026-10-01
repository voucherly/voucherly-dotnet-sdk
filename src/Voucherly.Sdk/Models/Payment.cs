using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// The Payment object.
/// </summary>
public class Payment : VoucherlyObject
{
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="TenantMode"/> values.</remarks>
    public string? Tenant { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the merchant this Payment belongs to, that is your own merchant.
    /// </summary>
    public string? MerchantId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A unique string to reference the Payment. This can be a customer ID, a cart ID, or similar, and can be used to reconcile the Payment with your internal systems.
    /// </summary>
    public string? ReferenceId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The identifier of the order this Payment settles in your own systems.
    /// </summary>
    public string? ExternalOrderId { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="PaymentMode"/> values.</remarks>
    public string? Mode { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the parent Payment, if this Payment is a child payment (e.g., a wallet).
    /// </summary>
    public string? ParentPaymentId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Company this Payment is associated with.
    /// </summary>
    public string? CompanyId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Store this Payment belongs to.
    /// </summary>
    public string? StoreId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Customer of this Payment.
    /// </summary>
    public string? CustomerId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The email address of the customer associated with this Payment. Required when <c>customerId</c> is not provided.
    /// </summary>
    public string? CustomerEmail { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The first name of the customer associated with this Payment.
    /// </summary>
    public string? CustomerFirstName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The last name of the customer associated with this Payment.
    /// </summary>
    public string? CustomerLastName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The phone number of the customer associated with this Payment.
    /// </summary>
    public string? CustomerPhoneNumber { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the PaymentGateway preselected for the first transaction.
    /// </summary>
    public string? SelectedPaymentGateway { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the PaymentGatewayAccount used to process this Payment.
    /// </summary>
    public string? PaymentGatewayConfigurationId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The URL where the customer should be redirected to complete the payment. This URL is provided after creating a Payment and should be used to redirect the customer to the Voucherly checkout page.
    /// </summary>
    public string? CheckoutUrl { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The URL Voucherly calls server to server when the Payment changes status.
    /// </summary>
    public string? CallbackUrl { get; set { field = value; MarkAssigned(); } }

    public PaymentCloseCheckout? CloseCheckout { get; set { field = value; MarkAssigned(); } }

    public PaymentLastCallback? Callback { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The total amount of the payment before discounts, in cents. This is the sum of all line items.
    /// </summary>
    public int TotalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The total amount of discounts applied to the payment, in cents. This is the sum of all discount amounts.
    /// </summary>
    public int DiscountAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The final amount to be paid after applying all discounts, in cents. This is calculated as totalAmount - discountAmount.
    /// </summary>
    public int FinalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The part of finalAmount that meal vouchers can pay, in cents. It is the sum of the food lines after their discounts, capped at finalAmount.
    /// </summary>
    public int FoodAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The total amount that has been paid so far, in cents. This includes both regular payments and voucher payments.
    /// </summary>
    public int PaidAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount that has been paid with a digital payment method, in cents.
    /// </summary>
    public int PaidDigitalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount that has been paid using vouchers, in cents.
    /// </summary>
    public int PaidVoucherAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount that has been paid with a fringe benefit credit, in cents.
    /// </summary>
    public int PaidFringeAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount that has been paid in cash, in cents.
    /// </summary>
    public int PaidCashAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The remaining amount to be paid, in cents. This is calculated as finalAmount - paidAmount.
    /// </summary>
    public int Amount { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="PaymentStatus"/> values.</remarks>
    public string? Status { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount that has been confirmed (captured), in cents.
    /// </summary>
    public int ConfirmedAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The date and time the Payment was confirmed, in UTC.
    /// </summary>
    public DateTimeOffset? ConfirmedOnUtc { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount that has been cancelled, in cents.
    /// </summary>
    public int CancelledAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount that has been refunded, in cents.
    /// </summary>
    public int RefundedAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The date and time the Payment was refunded, in UTC.
    /// </summary>
    public DateTimeOffset? RefundedOnUtc { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Receipt issued for this Payment.
    /// </summary>
    public string? ReceiptId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Receipt issued for the refund of this Payment.
    /// </summary>
    public string? RefundReceiptId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The Receipt issued for this Payment.
    /// </summary>
    public Receipt? Receipt { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The Receipt issued for the refund of this Payment.
    /// </summary>
    public Receipt? RefundReceipt { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The date and time the Payment was created, in UTC.
    /// </summary>
    public DateTimeOffset Created { get; set { field = value; MarkAssigned(); } }

    public Dictionary<string, string>? Metadata { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// An array of Transaction objects representing all payment attempts and transactions associated with this Payment.
    /// </summary>
    public List<Transaction>? Transactions { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// An array of PaymentDiscount objects representing all discounts applied to this Payment.
    /// </summary>
    public List<PaymentDiscount>? Discounts { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// An array of PaymentLine objects representing the line items (products or services) included in this Payment.
    /// </summary>
    public List<PaymentLine>? Lines { get; set { field = value; MarkAssigned(); } }
}
