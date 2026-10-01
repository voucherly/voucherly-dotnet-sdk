namespace Voucherly.Sdk.Enums;

/// <summary>
/// Has the value "live" if the object exists in live mode, or "sand" if it exists in test mode.
/// </summary>
public static class TenantMode
{
    public const string Sand = "sand";

    public const string Live = "live";
}
