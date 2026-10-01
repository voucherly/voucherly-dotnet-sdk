namespace Voucherly.Sdk.Models;

public class VolumesReport : VoucherlyObject
{
    /// <summary>
    /// An array containing the per-PaymentGateway breakdown rows, paginated by any request parameters.
    /// </summary>
    public List<RevenuesDetails>? Items { get; set { field = value; MarkAssigned(); } }

    public Pagination? Pagination { get; set { field = value; MarkAssigned(); } }

    public RevenuesTotals? Totals { get; set { field = value; MarkAssigned(); } }
}
