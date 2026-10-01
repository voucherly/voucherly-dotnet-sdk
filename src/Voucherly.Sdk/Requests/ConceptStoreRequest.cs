namespace Voucherly.Sdk.Requests;

/// <summary>
/// The writable fields of a Concept Store. Every field is replaced on update; omitted optional fields are cleared.
/// </summary>
public class ConceptStoreRequest : VoucherlyObject
{
    /// <summary>
    /// The display name of the Concept Store, unique across the Merchant. Leading and trailing whitespace is removed.
    /// </summary>
    public string Name { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// Your own identifier for the Concept Store, unique across the Merchant. Leading and trailing whitespace is removed; a blank value is stored as null.
    /// </summary>
    public string? ExternalId1 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A second identifier of your own for the Concept Store, unique across the Merchant. Leading and trailing whitespace is removed; a blank value is stored as null.
    /// </summary>
    public string? ExternalId2 { get; set { field = value; MarkAssigned(); } }
}
