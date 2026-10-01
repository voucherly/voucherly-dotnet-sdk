using Voucherly.Sdk.Http;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk.Services;

public interface IStoreAreaService
{
    /// <summary>
    /// List all Store Areas.
    /// </summary>
    Task<Page<StoreArea>> ListAsync(ListStoreAreaParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a Store Area.
    /// </summary>
    Task<StoreArea> CreateAsync(StoreAreaRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a Store Area.
    /// </summary>
    Task<StoreArea> RetrieveAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a Store Area.
    /// </summary>
    Task<StoreArea> UpdateAsync(string id, StoreAreaRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a Store Area.
    /// Permanently deletes the Store Area.
    /// The Stores that referenced it are never deleted: pass <c>migrateToStoreAreaId</c> to reassign them to another Store Area, or omit it to leave them without one (their <c>storeAreaId</c> becomes null).
    /// Call <c>GET /v1/stores?storeAreaId={id}</c> first to list the Stores this operation will affect.
    /// </summary>
    Task DeleteAsync(string id, DeleteStoreAreaParams? parameters = null, CancellationToken cancellationToken = default);
}

internal sealed class StoreAreaService(ApiRequestor requestor) : ServiceBase(requestor), IStoreAreaService
{
    public Task<Page<StoreArea>> ListAsync(ListStoreAreaParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Page<StoreArea>>(HttpMethod.Get, "/v1/store_areas", null, parameters, cancellationToken);

    public Task<StoreArea> CreateAsync(StoreAreaRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<StoreArea>(HttpMethod.Post, "/v1/store_areas", request, null, cancellationToken);
    }

    public Task<StoreArea> RetrieveAsync(string id, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<StoreArea>(HttpMethod.Get, Path("/v1/store_areas/{0}", id), null, null, cancellationToken);

    public Task<StoreArea> UpdateAsync(string id, StoreAreaRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<StoreArea>(HttpMethod.Put, Path("/v1/store_areas/{0}", id), request, null, cancellationToken);
    }

    public Task DeleteAsync(string id, DeleteStoreAreaParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendNoContentAsync(HttpMethod.Delete, Path("/v1/store_areas/{0}", id), parameters, cancellationToken);
}
