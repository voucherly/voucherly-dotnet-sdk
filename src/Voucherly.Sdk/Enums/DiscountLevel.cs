namespace Voucherly.Sdk.Enums;

/// <summary>
/// <c>Line</c> when the discount is already in the <c>discountAmount</c> of the lines it applies to, <c>Subtotal</c> when it comes off the sum of the lines and leaves them as they are.
/// </summary>
public static class DiscountLevel
{
    public const string Line = "Line";

    public const string Subtotal = "Subtotal";
}
