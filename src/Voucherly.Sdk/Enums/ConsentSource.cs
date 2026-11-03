namespace Voucherly.Sdk.Enums;

/// <summary>
/// Where the choice was collected. Defaults to <c>Other</c>.
/// </summary>
public static class ConsentSource
{
    public const string Signup = "Signup";

    public const string Profile = "Profile";

    public const string Checkout = "Checkout";

    public const string Reservation = "Reservation";

    public const string LiveCapture = "LiveCapture";

    public const string Import = "Import";

    public const string Staff = "Staff";

    public const string Other = "Other";
}
