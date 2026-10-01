namespace Voucherly.Sdk.Models;

/// <summary>
/// The postal address of the Store.
/// </summary>
public class StoreLocation : VoucherlyObject
{
    /// <summary>
    /// The street name.
    /// </summary>
    public string? StreetName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The street number.
    /// </summary>
    public string? StreetNumber { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The city.
    /// </summary>
    public string? City { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The two-letter province code.
    /// </summary>
    public string? Province { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The postal code.
    /// </summary>
    public string? PostalCode { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The country.
    /// </summary>
    public string? Country { get; set { field = value; MarkAssigned(); } }
}
