using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// Essential information about a PaymentGateway, used when embedded in other resources (e.g., Transactions, Terminals).
/// </summary>
public class PaymentGatewayEssential : VoucherlyObject
{
    /// <summary>
    /// Unique identifier for the PaymentGateway.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The display name of the PaymentGateway (e.g., "Visa", "Mastercard", "Edenred").
    /// </summary>
    public string? Name { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// URL of the image to display for this PaymentGateway.
    /// </summary>
    public string? Image { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Null when the PaymentGateway is no longer offered and only survives on historical records.
    /// </summary>
    /// <remarks>One of the <see cref="PaymentGatewayType"/> values.</remarks>
    public string? Type { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Null when the PaymentGateway is no longer offered and only survives on historical records.
    /// </summary>
    /// <remarks>One of the <see cref="CheckoutAction"/> values.</remarks>
    public string? PaymentAction { get; set { field = value; MarkAssigned(); } }
}
