using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Voucherly.Sdk;

/// <summary>
/// Base of every object sent to or received from the API.
/// Only the properties that have been assigned are serialized, an explicit null included, so a request never sends a field its caller did not set.
/// </summary>
public abstract class VoucherlyObject
{
    private readonly HashSet<string> _assigned = [];

    /// <summary>
    /// The members of the JSON object that this version of the SDK does not know.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    internal bool IsAssigned(string propertyName) => _assigned.Contains(propertyName);

    protected void MarkAssigned([CallerMemberName] string propertyName = "") => _assigned.Add(propertyName);
}
