namespace Voucherly.Sdk.Models;

public class Page<T> : VoucherlyObject
    where T : VoucherlyObject
{
    /// <summary>
    /// An array containing the actual response elements, paginated by any request parameters.
    /// </summary>
    public List<T> Items { get; set { field = value; MarkAssigned(); } } = [];

    public Pagination? Pagination { get; set { field = value; MarkAssigned(); } }
}
