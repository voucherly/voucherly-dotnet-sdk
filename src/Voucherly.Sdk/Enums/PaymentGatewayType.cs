namespace Voucherly.Sdk.Enums;

/// <summary>
/// The type of payment gateway.
/// </summary>
public static class PaymentGatewayType
{
    /// <summary>
    /// A meal voucher payment gateway, such as Edenred or Pluxee.
    /// </summary>
    public const string MealVoucher = "MealVoucher";

    /// <summary>
    /// A card payment gateway, for credit and debit cards.
    /// </summary>
    public const string CC = "CC";

    /// <summary>
    /// Any other payment gateway, such as Klarna or Apple Pay and Google Pay.
    /// </summary>
    public const string Other = "Other";

    /// <summary>
    /// A payment method of Voucherly itself, such as the wallet or the prepaid balance.
    /// </summary>
    public const string Custom = "Custom";

    /// <summary>
    /// A payment gateway that is never offered at checkout, such as manual payments.
    /// </summary>
    public const string Hidden = "Hidden";
}
