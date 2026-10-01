namespace Voucherly.Sdk.Requests;

public sealed class ListStoreAreaParams : RequestParams
{
    /// <summary>
    /// Filter by Store Areas whose name contains this value. The match is case-sensitive.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Filter by a specific set of Store Area identifiers.
    /// </summary>
    public IReadOnlyList<string>? Ids { get; set; }

    /// <summary>
    /// A limit on the number of objects to be returned. Limit can range between 1 and 100, and the default is 10.
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// A cursor for pagination across multiple pages of results. Don’t include this parameter on the first call. Use the <c>nextStart</c> value returned in a previous response to request subsequent results.
    /// </summary>
    public string? Start { get; set; }
}
