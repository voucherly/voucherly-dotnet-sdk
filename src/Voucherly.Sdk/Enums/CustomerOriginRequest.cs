namespace Voucherly.Sdk.Enums;

/// <summary>
/// Where you won the Customer. Voucherly records the other points of acquisition, such as the table or a Payment link, itself, and refuses them here with code <c>ORIGIN_NOT_DECLARABLE</c>. Defaults to <c>Api</c>.
/// </summary>
public static class CustomerOriginRequest
{
    public const string Site = "Site";

    public const string Staff = "Staff";

    public const string Import = "Import";

    public const string Api = "Api";
}
