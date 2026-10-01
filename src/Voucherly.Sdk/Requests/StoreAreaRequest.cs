namespace Voucherly.Sdk.Requests;

/// <summary>
/// The writable fields of a Store Area.
/// </summary>
public class StoreAreaRequest : VoucherlyObject
{
    /// <summary>
    /// The display name of the Store Area, unique across the Merchant. Leading and trailing whitespace is removed.
    /// </summary>
    public string Name { get; set { field = value; MarkAssigned(); } } = null!;
}
