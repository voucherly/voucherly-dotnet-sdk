namespace Voucherly.Sdk.Models;

/// <summary>
/// Response object containing a list of available PaymentGateways.
/// </summary>
public class PaymentGatewayList : VoucherlyObject
{
    /// <summary>
    /// An array of PaymentGateway objects representing all available PaymentGateways.
    /// </summary>
    public List<PaymentGateway>? Items { get; set { field = value; MarkAssigned(); } }
}
