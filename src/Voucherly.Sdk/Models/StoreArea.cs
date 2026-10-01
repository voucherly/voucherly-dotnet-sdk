namespace Voucherly.Sdk.Models;

/// <summary>
/// A Store Area is a geographical or operational grouping of Stores, defined by the Merchant.
/// </summary>
public class StoreArea : VoucherlyObject
{
    /// <summary>
    /// Unique identifier for the Store Area.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The display name of the Store Area, unique across the Merchant.
    /// </summary>
    public string? Name { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp when the Store Area was created.
    /// </summary>
    public DateTimeOffset CreatedOnUtc { get; set { field = value; MarkAssigned(); } }
}
