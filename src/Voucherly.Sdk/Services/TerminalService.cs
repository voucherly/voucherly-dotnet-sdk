using Voucherly.Sdk.Http;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk.Services;

public interface ITerminalService
{
    /// <summary>
    /// List all Terminals.
    /// </summary>
    Task<Page<Terminal>> ListAsync(ListTerminalParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a Terminal.
    /// </summary>
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}

internal sealed class TerminalService(ApiRequestor requestor) : ServiceBase(requestor), ITerminalService
{
    public Task<Page<Terminal>> ListAsync(ListTerminalParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Page<Terminal>>(HttpMethod.Get, "/v1/terminals", null, parameters, cancellationToken);

    public Task DeleteAsync(string id, CancellationToken cancellationToken = default) =>
        Requestor.SendNoContentAsync(HttpMethod.Delete, Path("/v1/terminals/{0}", id), null, cancellationToken);
}
