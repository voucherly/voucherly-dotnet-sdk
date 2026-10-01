using Voucherly.Sdk.Enums;
using Voucherly.Sdk.Models;

namespace Voucherly.Sdk.Requests;

/// <summary>
/// Request body for creating a Payment.
/// </summary>
public class CreatePaymentRequest : VoucherlyObject
{
    /// <summary>
    /// A unique string to reference the Payment. This can be a customer ID, a cart ID, or similar, and can be used to reconcile the Payment with your internal systems.
    /// </summary>
    public string? ReferenceId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The date this Payment refers to in your own system, when it differs from the date it is created on.
    /// </summary>
    public DateOnly? ReferenceDate { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="PaymentMode"/> values.</remarks>
    public string? Mode { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Company this Payment is associated with. If empty, the Company associated with the Customer will be used, if one exists.
    /// You cannot specify both <c>customerId</c> and <c>companyId</c> at the same time.
    /// </summary>
    public string? CompanyId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Customer of this Payment. If empty a new Customer will be created.
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
    /// The ID of the Customer's PaymentMethod to attach to this Payment.
    /// </summary>
    public string? CustomerPaymentMethodId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Default PaymentGateway used for the first transaction.
    /// </summary>
    public string? SelectedPaymentGatewayId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the PaymentGatewayConfiguration to use for this Payment, in the form <c>pgc_01kg2gestgepdbmsn7hs6bsrwp</c>. Use it when the same gateway is configured more than once and you need a specific configuration. When omitted, the configuration is resolved in this order: the default configuration of the API key used for the request, then the one of the Store the Payment refers to, then the Merchant default configuration.
    /// </summary>
    public string? PaymentGatewayConfigurationId { get; set { field = value; MarkAssigned(); } }

    public Dictionary<string, string>? Metadata { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// An array of PaymentLine objects representing the line items (products or services) to include in this Payment. Each line must reference a product either by <c>productId</c>, by an inline <c>product</c> object, or both (in which case the inline <c>product</c> overrides the referenced Product configuration).
    /// </summary>
    public List<PaymentLineRequest>? Lines { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// An array of PaymentDiscount objects representing all discounts applied to this Payment.
    /// </summary>
    public List<PaymentDiscount>? Discounts { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Seconds the customer has to complete the checkout, after which the Payment is voided. Without it the checkout has no deadline of its own, and a Payment still <c>Requested</c> 12 hours after its creation is voided.
    /// </summary>
    public int? Timeout { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// When the Payment counts as paid. Defaults to <c>Standard</c>. With <c>Standard</c> the Payment stays open until its whole amount is paid, so the customer can combine several payment gateways, such as meal vouchers and a card for the rest. With <c>Partial</c> the first successful transaction completes the Payment, whatever it paid: <c>amount</c> is set to the paid amount, and anything left is settled outside this Payment. A failed transaction leaves the Payment open for another attempt. With <c>AnyTransaction</c> the Payment gets a single attempt: it completes as with <c>Partial</c> when the first transaction succeeds, and is voided when it fails. Use it on a card reader, where a declined card has to end the Payment. With <c>Partial</c> and <c>AnyTransaction</c> the checkout does not capture the transactions it authorized, so confirm the Payment or set <c>isAutoConfirm</c>. Both require a subscription plan that includes partial payments, except in sandbox.
    /// </summary>
    /// <remarks>One of the <see cref="CompletionMode"/> values.</remarks>
    public string? CompletionMode { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of an existing store, in the form <c>sto_01jkqh8ksaeszr3kme0vcq0hbz</c>. Use <c>store</c> instead to identify it by your own identifiers.
    /// </summary>
    public string? StoreId { get; set { field = value; MarkAssigned(); } }

    public CreatePaymentRequestStore? Store { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Transactions to prepare on the Payment, each bound to a specific payment gateway. Use it to charge a gateway server-side instead of letting the customer pick one on the Checkout page.
    /// </summary>
    public List<CreatePaymentRequestTransaction>? Transactions { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Enable or disable auto confirm.
    /// If you don't specify anything the default Merchant configuration will be used.
    /// When auto confirm is enabled, payment is automatically confirmed before redirecting customer to RedirectSuccessUrl.
    /// When auto confirm is disabled, payment requires manual confirmation.
    /// </summary>
    public bool? IsAutoConfirm { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// What to do when the amount to charge is below the minimum a payment gateway accepts — 0.50 EUR on most card gateways. The request is raised to that minimum, and this decides what happens to the part that exceeds what is due: with <c>None</c> it stays with you, with <c>Credit</c> it is credited to the customer's wallet. With <c>HideGateways</c> the amount is not raised at all: the gateway is not offered on the Checkout page, and a transaction requested on it fails with <c>AMOUNT_BELOW_MINIMUM</c>. Use <c>HideGateways</c> when nobody can pick another method, such as a charge on a terminal. If you don't specify anything the default Merchant configuration will be used.
    /// </summary>
    /// <remarks>One of the <see cref="ExceedingAmountMode"/> values.</remarks>
    public string? ExceedingAmountMode { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The URL to which Voucherly should send customers when payment is complete.
    /// </summary>
    public string RedirectOkUrl { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The URL to which Voucherly should send customers when payment is cancelled or failed.
    /// </summary>
    public string RedirectKoUrl { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The URL where Voucherly will send a Server-to-Server (S2S) callback notification when the payment status changes. This allows your system to be notified synchronously about payment updates.
    /// </summary>
    public string? CallbackUrl { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The language of Voucherly Checkout page.
    /// </summary>
    public string? Language { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The country of the purchase. If different from Italy, voucher payment gateways are not shown.
    /// </summary>
    public string? Country { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Purchase shipping address.
    /// </summary>
    public string? ShippingAddress { get; set { field = value; MarkAssigned(); } }
}
