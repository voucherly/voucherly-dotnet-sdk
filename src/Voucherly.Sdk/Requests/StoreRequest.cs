using Voucherly.Sdk.Models;

namespace Voucherly.Sdk.Requests;

/// <summary>
/// The writable fields of a Store. Every field is replaced on update; omitted optional fields are cleared.
/// </summary>
public class StoreRequest : VoucherlyObject
{
    /// <summary>
    /// The ID of the Concept Store this Store belongs to. It must belong to the authenticated Merchant.
    /// </summary>
    public string? ConceptStoreId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Store Area this Store belongs to. It must belong to the authenticated Merchant.
    /// </summary>
    public string? StoreAreaId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The display name of the Store. Leading and trailing whitespace is removed.
    /// </summary>
    public string Name { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// A URL-safe identifier for the Store, unique across the Merchant. Leading and trailing whitespace is removed; a blank value is stored as null.
    /// </summary>
    public string? Slug { get; set { field = value; MarkAssigned(); } }

    public StorePos? Pos { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Your own identifier for the Store, unique across the Merchant. Required when <c>pos</c> is set. Leading and trailing whitespace is removed; a blank value is stored as null.
    /// </summary>
    public string? ExternalId1 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A second identifier of your own for the Store, unique across the Merchant. Leading and trailing whitespace is removed; a blank value is stored as null.
    /// </summary>
    public string? ExternalId2 { get; set { field = value; MarkAssigned(); } }

    public StoreLocation? Address { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the Store is operational. Unlike the other optional fields, omitting it does not clear it but defaults it to true, so an update that leaves it out reactivates the Store.
    /// </summary>
    public bool IsActive { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the PaymentGatewayConfiguration used by this Store. It must belong to the authenticated Merchant and be active. When null, the Merchant default configuration applies.
    /// </summary>
    public string? PaymentGatewayConfigurationId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the Store accepts e-commerce orders.
    /// </summary>
    public bool EcommerceIsActive { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the Store accepts e-commerce orders for delivery.
    /// </summary>
    public bool EcommerceDeliveryIsActive { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the Store accepts e-commerce orders for pickup.
    /// </summary>
    public bool EcommercePickupIsActive { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the Store accepts e-commerce reservations.
    /// </summary>
    public bool EcommerceReservationIsActive { get; set { field = value; MarkAssigned(); } }
}
