namespace Voucherly.Sdk.Models;

/// <summary>
/// A Store is a point of sale of a Merchant, with its own address, POS connection and payment configuration.
/// </summary>
public class Store : VoucherlyObject
{
    /// <summary>
    /// Unique identifier for the Store.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Concept Store this Store belongs to.
    /// </summary>
    public string? ConceptStoreId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The name of the Concept Store this Store belongs to. Returned only when <c>conceptStore</c> is requested through the <c>include</c> parameter. Retrieve the Concept Store from <c>/v1/concept_stores/{conceptStoreId}</c> to read its other fields.
    /// </summary>
    public string? ConceptStoreName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Store Area this Store belongs to.
    /// </summary>
    public string? StoreAreaId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The name of the Store Area this Store belongs to. Returned only when <c>storeArea</c> is requested through the <c>include</c> parameter. Retrieve the Store Area from <c>/v1/store_areas/{storeAreaId}</c> to read its other fields.
    /// </summary>
    public string? StoreAreaName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The display name of the Store.
    /// </summary>
    public string? Name { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A URL-safe identifier for the Store, unique across the Merchant.
    /// </summary>
    public string? Slug { get; set { field = value; MarkAssigned(); } }

    public StorePos? Pos { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Your own identifier for the Store, unique across the Merchant.
    /// </summary>
    public string? ExternalId1 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A second identifier of your own for the Store, unique across the Merchant.
    /// </summary>
    public string? ExternalId2 { get; set { field = value; MarkAssigned(); } }

    public StoreLocation? Address { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the Store is operational.
    /// </summary>
    public bool IsActive { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the PaymentGatewayConfiguration used by this Store. When null, the Merchant default configuration applies.
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

    /// <summary>
    /// The current health of the Store POS connection. Returned only when <c>status</c> is requested through the <c>include</c> parameter, and only for active Stores that have <c>pos</c> configured.
    /// </summary>
    public StoreStatus? Status { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The UTC timestamp when the Store was created.
    /// </summary>
    public DateTimeOffset CreatedOnUtc { get; set { field = value; MarkAssigned(); } }
}
