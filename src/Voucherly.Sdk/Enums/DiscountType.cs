namespace Voucherly.Sdk.Enums;

/// <summary>
/// Defines how the discount is calculated:
/// - <c>DOLLAROFF</c>: A fixed amount in cents is subtracted from the total.
/// - <c>PERCENOFF</c>: A percentage of the total amount is subtracted.
/// - <c>FIXED</c>: The final price is set to a fixed amount, overriding the original total.
/// </summary>
public static class DiscountType
{
    public const string DOLLAROFF = "DOLLAROFF";

    public const string PERCENOFF = "PERCENOFF";

    public const string FIXED = "FIXED";
}
