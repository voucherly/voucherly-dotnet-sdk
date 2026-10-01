namespace Voucherly.Sdk.Models;

public class Pagination : VoucherlyObject
{
    /// <summary>
    /// Whether or not there are more elements available after this set. If false, this set comprises the end of the list.
    /// </summary>
    public bool HasMore { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A cursor for use in pagination. If <c>hasMore</c> is true, you can pass the value of <c>nextStart</c> to a subsequent call to fetch the next page of results.
    /// </summary>
    public string? NextStart { get; set { field = value; MarkAssigned(); } }
}
