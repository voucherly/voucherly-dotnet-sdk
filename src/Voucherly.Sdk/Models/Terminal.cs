using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// A POS Terminal registered with a PaymentGateway and optionally bound to a Store.
/// </summary>
public class Terminal : VoucherlyObject
{
    /// <summary>
    /// Unique identifier for the Terminal.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the PaymentGatewayAccount this Terminal belongs to.
    /// </summary>
    public string? PaymentGatewayAccountId { get; set { field = value; MarkAssigned(); } }

    public PaymentGatewayEssential? PaymentGateway { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Store this Terminal is bound to, when applicable.
    /// </summary>
    public string? StoreId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The name of the Store this Terminal is bound to, when applicable.
    /// </summary>
    public string? StoreName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The identifier assigned to the Terminal by the PaymentGateway provider.
    /// </summary>
    public string? ExternalId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The identifier you assigned to the Terminal to reconcile it with your own systems.
    /// </summary>
    public string? ReferenceId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The display name of the Terminal.
    /// </summary>
    public string? Name { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The brand of the physical device.
    /// </summary>
    public string? Brand { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The model of the physical device.
    /// </summary>
    public string? Model { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The serial number of the physical device.
    /// </summary>
    public string? SerialNumber { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="TerminalStatus"/> values.</remarks>
    public string? Status { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp when the Terminal was created in the PaymentGateway provider system.
    /// </summary>
    public DateTimeOffset ExternalCreatedAt { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp when the Terminal was last updated in the PaymentGateway provider system.
    /// </summary>
    public DateTimeOffset? ExternalUpdatedAt { get; set { field = value; MarkAssigned(); } }
}
