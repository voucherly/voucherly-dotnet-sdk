namespace Voucherly.Sdk.Models;

/// <summary>
/// An address associated with a Company, typically used for delivery purposes.
/// </summary>
public class CompanyAddress : VoucherlyObject
{
    /// <summary>
    /// Unique identifier for the address.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A friendly label or name for this address (e.g., "Main Office", "Warehouse").
    /// </summary>
    public string? Label { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The street name of the address.
    /// </summary>
    public string? StreetName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The street number of the address.
    /// </summary>
    public string? StreetNumber { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The city of the address.
    /// </summary>
    public string? City { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The province or state of the address.
    /// </summary>
    public string? Province { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The postal or ZIP code of the address.
    /// </summary>
    public string? PostalCode { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The country code of the address (e.g., "IT" for Italy).
    /// </summary>
    public string? Country { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The delivery fee amount for this address, in cents.
    /// </summary>
    public int DeliveryFeeAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The unique identifier of the store associated with this address.
    /// </summary>
    public string? StoreId { get; set { field = value; MarkAssigned(); } }
}
