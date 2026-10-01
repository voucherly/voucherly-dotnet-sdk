namespace Voucherly.Sdk.Models;

/// <summary>
/// The connection to the POS system that serves the Store. Setting it makes <c>externalId1</c> required.
/// </summary>
public class StorePos : VoucherlyObject
{
    /// <summary>
    /// The identifier of the Store inside the POS system.
    /// </summary>
    public string? PosId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The base URL of the POS system. A trailing slash is added when missing.
    /// </summary>
    public string? PosEndpoint { get; set { field = value; MarkAssigned(); } }
}
