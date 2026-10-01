using Voucherly.Sdk.Http;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk.Services;

public interface IConceptStoreService
{
    /// <summary>
    /// List all Concept Stores.
    /// </summary>
    Task<Page<ConceptStore>> ListAsync(ListConceptStoreParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a Concept Store.
    /// </summary>
    Task<ConceptStore> CreateAsync(ConceptStoreRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a Concept Store.
    /// </summary>
    Task<ConceptStore> RetrieveAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a Concept Store.
    /// Replaces every writable field of the Concept Store.
    /// Fields omitted from the request body are cleared, not preserved.
    /// </summary>
    Task<ConceptStore> UpdateAsync(string id, ConceptStoreRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a Concept Store.
    /// Permanently deletes the Concept Store.
    /// The Stores that referenced it are never deleted: pass <c>migrateToConceptStoreId</c> to reassign them to another Concept Store, or omit it to leave them without one (their <c>conceptStoreId</c> becomes null).
    /// Call <c>GET /v1/stores?conceptStoreId={id}</c> first to list the Stores this operation will affect.
    /// </summary>
    Task DeleteAsync(string id, DeleteConceptStoreParams? parameters = null, CancellationToken cancellationToken = default);
}

internal sealed class ConceptStoreService(ApiRequestor requestor) : ServiceBase(requestor), IConceptStoreService
{
    public Task<Page<ConceptStore>> ListAsync(ListConceptStoreParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Page<ConceptStore>>(HttpMethod.Get, "/v1/concept_stores", null, parameters, cancellationToken);

    public Task<ConceptStore> CreateAsync(ConceptStoreRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<ConceptStore>(HttpMethod.Post, "/v1/concept_stores", request, null, cancellationToken);
    }

    public Task<ConceptStore> RetrieveAsync(string id, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<ConceptStore>(HttpMethod.Get, Path("/v1/concept_stores/{0}", id), null, null, cancellationToken);

    public Task<ConceptStore> UpdateAsync(string id, ConceptStoreRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<ConceptStore>(HttpMethod.Put, Path("/v1/concept_stores/{0}", id), request, null, cancellationToken);
    }

    public Task DeleteAsync(string id, DeleteConceptStoreParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendNoContentAsync(HttpMethod.Delete, Path("/v1/concept_stores/{0}", id), parameters, cancellationToken);
}
