namespace Voucherly.Sdk.Enums;

/// <summary>
/// Where the PaymentLine comes from:
/// - <c>Standard</c>: the line was part of the original order.
/// - <c>TableUpsell</c>: the line was added by an upselling proposal accepted while paying at the table.
/// - <c>LiveOffer</c>: the product of the line was added to the cart from an upselling popup of Voucherly Marketing, in the session named by the <c>voucherly_marketing_session</c> metadata. Voucherly sets it when the payment is created.
/// </summary>
public static class LineOrigin
{
    public const string Standard = "Standard";

    public const string TableUpsell = "TableUpsell";

    public const string LiveOffer = "LiveOffer";
}
