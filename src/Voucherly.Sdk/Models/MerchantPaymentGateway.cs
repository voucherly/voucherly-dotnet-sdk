namespace Voucherly.Sdk.Models;

/// <summary>
/// Merchant-specific configuration for a payment gateway.
/// </summary>
public class MerchantPaymentGateway : VoucherlyObject
{
    /// <summary>
    /// The ID of the default payment gateway to use if no specific gateway is selected.
    /// </summary>
    public string? DefaultPaymentGatewayId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Indicates whether this payment gateway should be used as a fallback option when other gateways fail.
    /// </summary>
    public bool IsFallback { get; set { field = value; MarkAssigned(); } }
}
