using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// Where the Customer was won, set when it was created and never changed. Null for the Customers created before it was recorded.
/// </summary>
public class Origin : VoucherlyObject
{
    /// <summary>
    /// The point of acquisition. <c>Api</c> is a Customer your own system created without saying where it came from.
    /// </summary>
    /// <remarks>One of the <see cref="CustomerOriginType"/> values.</remarks>
    public string? Type { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The Store where the Customer was won, such as the Store of the table or of the reservation.
    /// </summary>
    public string? StoreId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of what brought the Customer, such as the Payment link or the reservation.
    /// </summary>
    public string? SourceId { get; set { field = value; MarkAssigned(); } }
}
