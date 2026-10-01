namespace Voucherly.Sdk.Enums;

/// <summary>
/// The packaging used by the Customers of this Company that do not set one of their own. Null when the Company follows the ecommerce site default.
/// </summary>
public static class PackagingType
{
    public const string Reusable = "Reusable";

    public const string Disposable = "Disposable";
}
