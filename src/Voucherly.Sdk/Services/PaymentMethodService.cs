using Voucherly.Sdk.Http;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk.Services;

public interface IPaymentMethodService
{
    /// <summary>
    /// List a Customer's PaymentMethods.
    /// Returns a paginated list of the PaymentMethods saved on the Customer, the most recent first.
    /// </summary>
    Task<Page<PaymentMethod>> ListAsync(string customerId, ListCustomerPaymentMethodParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a Customer's PaymentMethod.
    /// </summary>
    Task DeleteAsync(string customerId, string paymentMethodId, CancellationToken cancellationToken = default);
}

internal sealed class PaymentMethodService(ApiRequestor requestor) : ServiceBase(requestor), IPaymentMethodService
{
    public Task<Page<PaymentMethod>> ListAsync(string customerId, ListCustomerPaymentMethodParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Page<PaymentMethod>>(HttpMethod.Get, Path("/v1/customers/{0}/payment_methods", customerId), null, parameters, cancellationToken);

    public Task DeleteAsync(string customerId, string paymentMethodId, CancellationToken cancellationToken = default) =>
        Requestor.SendNoContentAsync(HttpMethod.Delete, Path("/v1/customers/{0}/payment_methods/{1}", customerId, paymentMethodId), null, cancellationToken);
}
