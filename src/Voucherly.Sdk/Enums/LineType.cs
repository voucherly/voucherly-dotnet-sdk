namespace Voucherly.Sdk.Enums;

/// <summary>
/// What the PaymentLine is, which decides both how the checkout lays it out and whether meal vouchers can pay it:
/// - <c>NonFood</c>: goods that are not food, such as detergents or household items. Not payable with meal vouchers, and the default when the line declares nothing.
/// - <c>Food</c>: grocery food. The only type meal vouchers can pay.
/// - <c>Shipping</c>: shipping costs. The checkout always shows it after the products, whatever position you send it in.
/// - <c>AdditionalCharge</c>: a delivery service, a tip or any other accessory charge. Shown after the products, like <c>Shipping</c>.
/// - <c>PreauthorizationMargin</c>: the margin authorised on top of the estimated total, covering variable-weight items and released once the effective amount is confirmed. The checkout does not list it among the products: it counts towards the total, and an info icon next to it explains the difference. Send <c>product.name</c> and <c>product.variant</c> on that line to replace the default wording of that explanation. At confirmation it is released whether you send it back or not: send it with quantity <c>0</c> or leave it out, anything else fails with <c>MARGIN_NOT_CONFIRMABLE</c>. It is never printed on the documents for the customer.
/// </summary>
public static class LineType
{
    public const string NonFood = "NonFood";

    public const string Food = "Food";

    public const string Shipping = "Shipping";

    public const string AdditionalCharge = "AdditionalCharge";

    public const string PreauthorizationMargin = "PreauthorizationMargin";
}
