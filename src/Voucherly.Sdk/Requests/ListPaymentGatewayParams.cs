using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Requests;

public sealed class ListPaymentGatewayParams : RequestParams
{
    /// <summary>
    /// Returns the PaymentGateways of the configuration the Store resolves to. Ignored when <c>paymentGatewayConfigurationId</c> is given.
    /// </summary>
    public string? StoreId { get; set; }

    /// <summary>
    /// Returns the PaymentGateways of one specific configuration, in the form <c>pgc_01kg2gestgepdbmsn7hs6bsrwp</c>. With neither this nor <c>storeId</c>, the default configuration of the merchant is used.
    /// </summary>
    public string? PaymentGatewayConfigurationId { get; set; }

    /// <summary>
    /// Specifies whether to return all payment gateways or only those that are enabled. If true, returns all payment gateways regardless of their enabled status. If false or omitted, returns only enabled payment gateways.
    /// </summary>
    public bool? All { get; set; }

    /// <summary>
    /// An array of nested objects to include in the response.
    /// </summary>
    /// <remarks>Each item is one of the <see cref="PaymentGatewayInclude"/> values.</remarks>
    public IReadOnlyList<string>? Include { get; set; }
}
