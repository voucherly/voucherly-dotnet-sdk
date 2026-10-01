namespace Voucherly.Sdk.Models;

/// <summary>
/// If this is a card PaymentMethod, this contains the user’s card details.
/// </summary>
public class CreditCardInfo : VoucherlyObject
{
    /// <summary>
    /// Card brand.
    /// </summary>
    public string? Brand { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Card masked PAN.
    /// </summary>
    public string? Pan { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The card's expiration date, typically in MM/YY format.
    /// </summary>
    public string? Expiration { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Two-digit number representing the card’s expiration month.
    /// </summary>
    public int ExpirationMonth { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Four-digit number representing the card’s expiration year.
    /// </summary>
    public int ExpirationYear { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The card product type (e.g., "Visa Classic", "Mastercard Gold").
    /// </summary>
    public string? Product { get; set { field = value; MarkAssigned(); } }
}
