namespace Voucherly.Sdk.Enums;

/// <summary>
/// Where the PaymentLine comes from:
/// - <c>Standard</c>: the line was part of the original order.
/// - <c>TableUpsell</c>: the line was added by an upselling proposal accepted while paying at the table.
/// </summary>
public static class LineOrigin
{
    public const string Standard = "Standard";

    public const string TableUpsell = "TableUpsell";
}
