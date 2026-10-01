using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// The outcome of the periodic health checks Voucherly runs against the Store POS connection.
/// </summary>
public class StoreStatus : VoucherlyObject
{
    /// <summary>
    /// The current health of the Store.
    /// </summary>
    /// <remarks>One of the <see cref="StoreStatusValue"/> values.</remarks>
    public string? Status { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp when the Store entered the current status.
    /// </summary>
    public DateTimeOffset CurrentStatusFromUtc { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp of the most recent health check.
    /// </summary>
    public DateTimeOffset LastCheckedAtUtc { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp of the most recent successful health check.
    /// </summary>
    public DateTimeOffset? LastSuccessAtUtc { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The number of consecutive failed health checks.
    /// </summary>
    public int ConsecutiveKos { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The number of consecutive successful health checks.
    /// </summary>
    public int ConsecutiveOks { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The response time of the most recent health check, in milliseconds.
    /// </summary>
    public int LastResponseTimeMs { get; set { field = value; MarkAssigned(); } }
}
