namespace Voucherly.Sdk.Requests;

public sealed class ListCustomerWalletMovementParams : RequestParams
{
    /// <summary>
    /// Lower bound (inclusive) of the movement timestamp filter, in UTC.
    /// </summary>
    public DateTimeOffset? FromDate { get; set; }

    /// <summary>
    /// Upper bound (inclusive) of the movement timestamp filter, in UTC.
    /// </summary>
    public DateTimeOffset? ToDate { get; set; }

    /// <summary>
    /// A limit on the number of objects to be returned. Limit can range between 1 and 100, and the default is 10.
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// A cursor for pagination across multiple pages of results. Don’t include this parameter on the first call. Use the <c>nextStart</c> value returned in a previous response to request subsequent results.
    /// </summary>
    public string? Start { get; set; }
}
