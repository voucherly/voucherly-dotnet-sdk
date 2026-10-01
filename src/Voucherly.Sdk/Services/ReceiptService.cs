using Voucherly.Sdk.Http;
using Voucherly.Sdk.Models;

namespace Voucherly.Sdk.Services;

public interface IReceiptService
{
    /// <summary>
    /// Retrieve a Receipt.
    /// </summary>
    Task<Receipt> RetrieveAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download a Receipt.
    /// Downloads the fiscal Receipt as a PDF document.
    /// </summary>
    Task<byte[]> DownloadAsync(string id, CancellationToken cancellationToken = default);
}

internal sealed class ReceiptService(ApiRequestor requestor) : ServiceBase(requestor), IReceiptService
{
    public Task<Receipt> RetrieveAsync(string id, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Receipt>(HttpMethod.Get, Path("/v1/receipts/{0}", id), null, null, cancellationToken);

    public Task<byte[]> DownloadAsync(string id, CancellationToken cancellationToken = default) =>
        Requestor.SendBytesAsync(Path("/v1/receipts/{0}/download", id), "application/pdf", cancellationToken);
}
