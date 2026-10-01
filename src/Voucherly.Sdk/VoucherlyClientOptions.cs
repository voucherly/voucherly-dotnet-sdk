using System.Reflection;
using System.Runtime.InteropServices;

namespace Voucherly.Sdk;

public sealed class VoucherlyClientOptions
{
    /// <summary>
    /// The secret key: <c>sk_sand_</c> for sandbox, <c>sk_live_</c> for live, or a platform key <c>ik_</c>.
    /// </summary>
    public string ApiKey { get; set; } = "";

    /// <summary>
    /// Declares the merchant a request operates on. Required with a platform key (<c>ik_</c>), which belongs to no merchant.
    /// </summary>
    public string? MerchantId { get; set; }

    /// <summary>
    /// Declares the environment a request operates on, <c>live</c> or <c>sand</c>. Required with a platform key that states no environment of its own.
    /// </summary>
    public string? Tenant { get; set; }

    public Uri BaseUrl { get; set; } = new("https://api.voucherly.it");

    public string? Os { get; set; } = RuntimeInformation.OSDescription.Replace(Environment.OSVersion.Version.ToString(), string.Empty).Trim();

    public string? OsVersion { get; set; } = Environment.OSVersion.Version.ToString();

    public string? OsFramework { get; set; } = RuntimeInformation.FrameworkDescription;

    public string? App { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;

    public string? AppVersion { get; set; } = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();

    public string? AppHouse { get; set; } = "Voucherly";

    public string? DeviceType { get; set; }
}
