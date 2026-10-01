using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// A fiscal Receipt issued for a Payment.
/// </summary>
public class Receipt : VoucherlyObject
{
    /// <summary>
    /// Unique identifier for the Receipt.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The identifier assigned to the Receipt by the upstream fiscal provider.
    /// </summary>
    public string? ExternalId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The fiscal Receipt number.
    /// </summary>
    public string? Number { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp when the Receipt was issued.
    /// </summary>
    public DateTimeOffset Date { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="ReceiptType"/> values.</remarks>
    public string? Type { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The total amount of the Receipt, in cents.
    /// </summary>
    public int Amount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the parent Receipt, when this Receipt is a void/refund of a previously issued Sale Receipt.
    /// </summary>
    public string? ParentReceiptId { get; set { field = value; MarkAssigned(); } }
}
