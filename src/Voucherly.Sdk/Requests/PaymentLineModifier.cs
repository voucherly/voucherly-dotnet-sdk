namespace Voucherly.Sdk.Requests;

/// <summary>
/// A modifier or customization applied to a payment line item (e.g., extra toppings, size options).
/// </summary>
public class PaymentLineModifier : VoucherlyObject
{
    /// <summary>
    /// The ID of the Modifier that this customization belongs to.
    /// </summary>
    public string ModifierId { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The ID of the Product that this customization belongs to.
    /// </summary>
    public string ProductId { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The quantity of this modifier.
    /// </summary>
    public int Quantity { get; set { field = value; MarkAssigned(); } }
}
