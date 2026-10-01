using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// Detailed information about a payment gateway, including merchant configuration and parameters.
/// </summary>
public class PaymentGateway : VoucherlyObject
{
    /// <summary>
    /// Unique identifier for the payment gateway.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The display name of the payment gateway (e.g., "Visa", "Mastercard", "Edenred").
    /// </summary>
    public string? Name { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The type of payment gateway.
    /// </summary>
    /// <remarks>One of the <see cref="PaymentGatewayType"/> values.</remarks>
    public string? Type { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The type of checkout action required by this payment gateway.
    /// </summary>
    /// <remarks>One of the <see cref="CheckoutAction"/> values.</remarks>
    public string? PaymentAction { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// URL of the image to display for this payment gateway.
    /// </summary>
    public string? Image { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// URL of the image to display for this payment gateway in the checkout page.
    /// </summary>
    public string? CheckoutImage { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// URL of the icon to display for this payment gateway.
    /// </summary>
    public string? Icon { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Indicates whether this payment gateway is currently active and available for use.
    /// </summary>
    public bool IsActive { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Merchant-specific configuration for this payment gateway.
    /// </summary>
    public MerchantPaymentGateway? MerchantConfiguration { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// An array of configuration parameters for this payment gateway.
    /// </summary>
    public List<PaymentGatewayParameter>? Parameters { get; set { field = value; MarkAssigned(); } }
}
