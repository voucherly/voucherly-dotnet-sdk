namespace Voucherly.Sdk.Models;

/// <summary>
/// A Concept Store is a brand used to group Stores of the same Merchant.
/// </summary>
public class ConceptStore : VoucherlyObject
{
    /// <summary>
    /// Unique identifier for the Concept Store.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The display name of the Concept Store, unique across the Merchant.
    /// </summary>
    public string? Name { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Your own identifier for the Concept Store, unique across the Merchant.
    /// </summary>
    public string? ExternalId1 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A second identifier of your own for the Concept Store, unique across the Merchant.
    /// </summary>
    public string? ExternalId2 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp when the Concept Store was created.
    /// </summary>
    public DateTimeOffset CreatedOnUtc { get; set { field = value; MarkAssigned(); } }
}
