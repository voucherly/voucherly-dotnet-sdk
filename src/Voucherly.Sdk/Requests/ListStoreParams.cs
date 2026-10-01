using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Requests;

public sealed class ListStoreParams : RequestParams
{
    /// <summary>
    /// Filter by Stores whose name contains this value. The match is case-sensitive.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Filter by Store management status. When omitted, both active and inactive Stores are returned.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Filter by the Stores assigned to this Concept Store.
    /// </summary>
    public string? ConceptStoreId { get; set; }

    /// <summary>
    /// Filter by the Stores assigned to this Store Area.
    /// </summary>
    public string? StoreAreaId { get; set; }

    /// <summary>
    /// Related data to embed in each Store. Omit to receive only the Store's own fields.
    /// </summary>
    /// <remarks>Each item is one of the <see cref="StoreInclude"/> values.</remarks>
    public IReadOnlyList<string>? Include { get; set; }

    /// <summary>
    /// A limit on the number of objects to be returned. Limit can range between 1 and 100, and the default is 10.
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// A cursor for pagination across multiple pages of results. Don’t include this parameter on the first call. Use the <c>nextStart</c> value returned in a previous response to request subsequent results.
    /// </summary>
    public string? Start { get; set; }
}
