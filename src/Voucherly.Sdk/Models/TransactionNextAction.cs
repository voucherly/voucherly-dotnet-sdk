using System.Text.Json;

namespace Voucherly.Sdk.Models;

/// <summary>
/// The action the payer still has to perform for the transaction to complete. Its shape depends on the PaymentGateway.
/// </summary>
public class TransactionNextAction : VoucherlyObject
{
    /// <summary>
    /// The gateway specific parameters describing the action.
    /// </summary>
    public Dictionary<string, JsonElement>? Params { get; set { field = value; MarkAssigned(); } }
}
