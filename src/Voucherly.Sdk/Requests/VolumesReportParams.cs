using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Requests;

public sealed class VolumesReportParams : RequestParams
{
    /// <summary>
    /// Start of the reporting period (inclusive). Only the date part is considered.
    /// </summary>
    public required DateTimeOffset FromDate { get; set; }

    /// <summary>
    /// End of the reporting period (inclusive). Only the date part is considered. Must be greater than or equal to <c>fromDate</c>, and the range cannot exceed 366 days.
    /// </summary>
    public required DateTimeOffset ToDate { get; set; }

    /// <summary>
    /// Filter by payment mode. When omitted, wallet recharges are excluded and only payments are aggregated.
    /// </summary>
    /// <remarks>One of the <see cref="PaymentMode"/> values.</remarks>
    public string? PaymentMode { get; set; }

    /// <summary>
    /// How rows are grouped. Repeat the parameter to group by more than one dimension (e.g. <c>groupBy=Store&amp;groupBy=Company</c>). When omitted, rows are aggregated per PaymentGateway only.
    /// </summary>
    /// <remarks>Each item is one of the <see cref="RevenuesGrouping"/> values.</remarks>
    public IReadOnlyList<string>? GroupBy { get; set; }

    /// <summary>
    /// Filter by one or more PaymentGateway identifiers. Repeat the parameter to provide multiple values.
    /// </summary>
    public IReadOnlyList<string>? PaymentGatewayIds { get; set; }

    /// <summary>
    /// Filter by one or more Store identifiers. Repeat the parameter to provide multiple values.
    /// </summary>
    public IReadOnlyList<string>? StoreIds { get; set; }

    /// <summary>
    /// Filter by one or more Company identifiers. Repeat the parameter to provide multiple values.
    /// </summary>
    public IReadOnlyList<string>? CompanyIds { get; set; }

    /// <summary>
    /// A limit on the number of objects to be returned. Limit can range between 1 and 100, and the default is 10.
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// A cursor for pagination across multiple pages of results. Don’t include this parameter on the first call. Use the <c>nextStart</c> value returned in a previous response to request subsequent results.
    /// </summary>
    public string? Start { get; set; }
}
