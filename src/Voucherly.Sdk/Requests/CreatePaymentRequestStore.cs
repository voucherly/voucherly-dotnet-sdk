namespace Voucherly.Sdk.Requests;

/// <summary>
/// The store the Payment belongs to, identified by your own identifiers. When no store matches, Voucherly creates one on the fly.
/// </summary>
public class CreatePaymentRequestStore : VoucherlyObject
{
    /// <summary>
    /// Your primary identifier for the store.
    /// </summary>
    public string? ExternalId1 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Your secondary identifier for the store.
    /// </summary>
    public string? ExternalId2 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The name of the store, used when Voucherly has to create it.
    /// </summary>
    public string? Name { get; set { field = value; MarkAssigned(); } }
}
