namespace Voucherly.Sdk.Requests;

/// <summary>
/// Request body for creating or updating a Customer address.
/// </summary>
public class CustomerAddressRequest : VoucherlyObject
{
    /// <summary>
    /// A friendly label or name for this address (e.g., "Home", "Work").
    /// </summary>
    public string Label { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The street name of the address.
    /// </summary>
    public string StreetName { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The street number of the address.
    /// </summary>
    public string StreetNumber { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The city of the address.
    /// </summary>
    public string City { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The province or state of the address.
    /// </summary>
    public string Province { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The postal or ZIP code of the address.
    /// </summary>
    public string PostalCode { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The country code of the address (e.g., "IT" for Italy).
    /// </summary>
    public string Country { get; set { field = value; MarkAssigned(); } } = null!;
}
