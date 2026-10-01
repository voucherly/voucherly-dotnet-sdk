using Voucherly.Sdk.Http;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk.Services;

public interface IPaymentGatewayService
{
    /// <summary>
    /// List all PaymentGateways.
    /// </summary>
    Task<PaymentGatewayList> ListAsync(ListPaymentGatewayParams? parameters = null, CancellationToken cancellationToken = default);
}

internal sealed class PaymentGatewayService(ApiRequestor requestor) : ServiceBase(requestor), IPaymentGatewayService
{
    public Task<PaymentGatewayList> ListAsync(ListPaymentGatewayParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<PaymentGatewayList>(HttpMethod.Get, "/v1/payment_gateways", null, parameters, cancellationToken);
}
