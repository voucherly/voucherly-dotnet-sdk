using System.Text.Json.Serialization;
using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// Information about the payment request, including the action required and any redirect URL. The remaining properties depend on the PaymentGateway named by <c>$type</c>.
/// </summary>
public class CheckoutPaymentRequest : VoucherlyObject
{
    /// <summary>
    /// The identifier of the PaymentGateway that produced the request, such as <c>stripe</c> or <c>satispay</c>. It tells you which gateway specific properties to expect alongside the documented ones.
    /// </summary>
    [JsonPropertyName("$type")]
    public string? Type { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The type of checkout action required to complete the payment.
    /// </summary>
    /// <remarks>One of the <see cref="CheckoutAction"/> values.</remarks>
    public string? Action { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The unique identifier of the transaction in the external payment gateway system.
    /// </summary>
    public string? ExternalTransactionId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The URL to redirect the customer to, if the action is REDIRECT. This URL is provided by the payment gateway.
    /// </summary>
    public string? Url { get; set { field = value; MarkAssigned(); } }
}
