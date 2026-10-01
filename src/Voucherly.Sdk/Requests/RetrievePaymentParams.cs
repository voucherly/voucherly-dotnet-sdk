using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Requests;

public sealed class RetrievePaymentParams : RequestParams
{
    /// <summary>
    /// An array of nested object to be included in response.
    /// </summary>
    /// <remarks>Each item is one of the <see cref="PaymentInclude"/> values.</remarks>
    public IReadOnlyList<string>? Include { get; set; }

    /// <summary>
    /// Seconds that the call will be hanging, waiting for a payment status change. Maximum value is 60 seconds.
    /// </summary>
    [HeaderParam("Voucherly-Wait-Time")]
    public int? WaitTime { get; set; }
}
