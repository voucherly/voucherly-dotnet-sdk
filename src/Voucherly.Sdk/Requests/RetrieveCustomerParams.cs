using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Requests;

public sealed class RetrieveCustomerParams : RequestParams
{
    /// <summary>
    /// An array of nested object to be included in response.
    /// </summary>
    /// <remarks>Each item is one of the <see cref="CustomerInclude"/> values.</remarks>
    public IReadOnlyList<string>? Include { get; set; }
}
