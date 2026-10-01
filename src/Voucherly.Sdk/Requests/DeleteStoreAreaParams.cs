namespace Voucherly.Sdk.Requests;

public sealed class DeleteStoreAreaParams : RequestParams
{
    /// <summary>
    /// The Store Area the referencing Stores are reassigned to. It must belong to the authenticated Merchant and must be different from <c>id</c>. When omitted, the referencing Stores are left without a Store Area.
    /// </summary>
    public string? MigrateToStoreAreaId { get; set; }
}
