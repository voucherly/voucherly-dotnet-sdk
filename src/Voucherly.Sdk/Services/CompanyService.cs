using Voucherly.Sdk.Http;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk.Services;

public interface ICompanyService
{
    /// <summary>
    /// Create a Company.
    /// </summary>
    Task<Company> CreateAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// List all Companies.
    /// </summary>
    Task<Page<Company>> ListAsync(ListCompanyParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a Company.
    /// </summary>
    /// <param name="idOrCode">The unique identifier (ID) or join code of the Company to retrieve.</param>
    /// <param name="parameters">The query parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<Company> RetrieveAsync(string idOrCode, RetrieveCompanyParams? parameters = null, CancellationToken cancellationToken = default);
}

internal sealed class CompanyService(ApiRequestor requestor) : ServiceBase(requestor), ICompanyService
{
    public Task<Company> CreateAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<Company>(HttpMethod.Post, "/v1/companys", request, null, cancellationToken);
    }

    public Task<Page<Company>> ListAsync(ListCompanyParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Page<Company>>(HttpMethod.Get, "/v1/companys", null, parameters, cancellationToken);

    public Task<Company> RetrieveAsync(string idOrCode, RetrieveCompanyParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Company>(HttpMethod.Get, Path("/v1/companys/{0}", idOrCode), null, parameters, cancellationToken);
}
