namespace Voucherly.Sdk.Requests;

/// <summary>
/// The product the line was created with, to pair it by product when the line is sent without its own ids.
/// </summary>
public class ConfirmPaymentRequestLineProduct : VoucherlyObject
{
    /// <summary>
    /// The external reference of the product, as sent in <c>product.externalId</c> at creation.
    /// </summary>
    public string? ExternalId { get; set { field = value; MarkAssigned(); } }

    public string? Name { get; set { field = value; MarkAssigned(); } }

    public string? Variant { get; set { field = value; MarkAssigned(); } }
}
