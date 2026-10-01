using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Requests;

public sealed class RetrieveStoreParams : RequestParams
{
    /// <summary>
    /// Related data to embed in the Store. Omit to receive only the Store's own fields.
    /// </summary>
    /// <remarks>Each item is one of the <see cref="StoreInclude"/> values.</remarks>
    public IReadOnlyList<string>? Include { get; set; }
}
