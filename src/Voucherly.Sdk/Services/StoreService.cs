using Voucherly.Sdk.Http;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk.Services;

public interface IStoreService
{
    /// <summary>
    /// List all Stores.
    /// </summary>
    Task<Page<Store>> ListAsync(ListStoreParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a Store.
    /// Creates a Store for the authenticated Merchant.
    /// <c>conceptStoreName</c>, <c>storeAreaName</c> and the <c>status</c> object are never returned by this operation; retrieve the Store with the <c>include</c> parameter to read them.
    /// </summary>
    Task<Store> CreateAsync(StoreRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a Store.
    /// </summary>
    Task<Store> RetrieveAsync(string id, RetrieveStoreParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a Store.
    /// Replaces every writable field of the Store.
    /// Fields omitted from the request body are cleared, not preserved.
    /// <c>conceptStoreName</c>, <c>storeAreaName</c> and the <c>status</c> object are never returned by this operation; retrieve the Store with the <c>include</c> parameter to read them.
    /// </summary>
    Task<Store> UpdateAsync(string id, StoreRequest request, CancellationToken cancellationToken = default);
}

internal sealed class StoreService(ApiRequestor requestor) : ServiceBase(requestor), IStoreService
{
    public Task<Page<Store>> ListAsync(ListStoreParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Page<Store>>(HttpMethod.Get, "/v1/stores", null, parameters, cancellationToken);

    public Task<Store> CreateAsync(StoreRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<Store>(HttpMethod.Post, "/v1/stores", request, null, cancellationToken);
    }

    public Task<Store> RetrieveAsync(string id, RetrieveStoreParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Store>(HttpMethod.Get, Path("/v1/stores/{0}", id), null, parameters, cancellationToken);

    public Task<Store> UpdateAsync(string id, StoreRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<Store>(HttpMethod.Put, Path("/v1/stores/{0}", id), request, null, cancellationToken);
    }
}
