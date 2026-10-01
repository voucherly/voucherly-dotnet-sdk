using Voucherly.Sdk.Http;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk.Services;

public interface IReportService
{
    /// <summary>
    /// Volumes.
    /// Returns aggregated turnover for the given date range, broken down by PaymentGateway and optionally by Store and/or Company, together with the grand totals across all rows.
    /// This is the same data shown in the Volumes report of the Voucherly Dashboard.
    /// </summary>
    Task<VolumesReport> VolumesAsync(VolumesReportParams parameters, CancellationToken cancellationToken = default);
}

internal sealed class ReportService(ApiRequestor requestor) : ServiceBase(requestor), IReportService
{
    public Task<VolumesReport> VolumesAsync(VolumesReportParams parameters, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        return Requestor.SendAsync<VolumesReport>(HttpMethod.Get, "/v1/reports/volumes", null, parameters, cancellationToken);
    }
}
