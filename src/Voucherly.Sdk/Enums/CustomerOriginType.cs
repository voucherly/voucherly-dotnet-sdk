namespace Voucherly.Sdk.Enums;

/// <summary>
/// The point of acquisition. <c>Api</c> is a Customer your own system created without saying where it came from.
/// </summary>
public static class CustomerOriginType
{
    public const string Site = "Site";

    public const string Table = "Table";

    public const string PaymentLink = "PaymentLink";

    public const string Reservation = "Reservation";

    public const string LoyaltyCard = "LoyaltyCard";

    public const string Collaborator = "Collaborator";

    public const string Popup = "Popup";

    public const string Import = "Import";

    public const string Staff = "Staff";

    public const string Api = "Api";
}
