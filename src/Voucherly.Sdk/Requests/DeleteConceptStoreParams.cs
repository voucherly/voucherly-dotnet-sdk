namespace Voucherly.Sdk.Requests;

public sealed class DeleteConceptStoreParams : RequestParams
{
    /// <summary>
    /// The Concept Store the referencing Stores are reassigned to. It must belong to the authenticated Merchant and must be different from <c>id</c>. When omitted, the referencing Stores are left without a Concept Store.
    /// </summary>
    public string? MigrateToConceptStoreId { get; set; }
}
