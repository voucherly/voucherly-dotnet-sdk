using Voucherly.Sdk.Http;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk.Services;

public interface ICustomerService
{
    /// <summary>
    /// Create a Customer.
    /// </summary>
    Task<Customer> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// List all Customers.
    /// </summary>
    Task<Page<Customer>> ListAsync(ListCustomerParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a Customer.
    /// </summary>
    Task<Customer> RetrieveAsync(string id, RetrieveCustomerParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a Customer.
    /// Updates the specified customer by setting the values of the parameters passed.
    /// Any parameters not provided will be left unchanged.
    /// </summary>
    Task<Customer> UpdateAsync(string id, UpdateCustomerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a Customer's prepaid balance.
    /// Returns the prepaid balance available to a Customer for a given date, based on the prepaid policy configured on the Customer's Company.
    /// </summary>
    Task<CustomerPrepaidBalance> RetrievePrepaidBalanceAsync(string customerId, RetrieveCustomerPrepaidBalanceParams parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// List a Customer's wallet movements.
    /// Returns a paginated list of wallet movements (credits, debits, refunds, adjustments) recorded on the Customer's wallet.
    /// </summary>
    Task<Page<CustomerWalletMovement>> ListWalletMovementsAsync(string id, ListCustomerWalletMovementParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// List a Customer's Addresses.
    /// Returns a paginated list of the Addresses of the Customer, the most recent first.
    /// </summary>
    Task<Page<CustomerAddress>> ListAddressesAsync(string customerId, ListCustomerAddressParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a Customer's Address.
    /// </summary>
    Task<CustomerAddress> CreateAddressAsync(string customerId, CustomerAddressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a Customer's Address.
    /// </summary>
    Task<CustomerAddress> RetrieveAddressAsync(string customerId, string addressId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a Customer's Address.
    /// </summary>
    Task<CustomerAddress> UpdateAddressAsync(string customerId, string addressId, CustomerAddressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a Customer's Address.
    /// </summary>
    Task DeleteAddressAsync(string customerId, string addressId, CancellationToken cancellationToken = default);
}

internal sealed class CustomerService(ApiRequestor requestor) : ServiceBase(requestor), ICustomerService
{
    public Task<Customer> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<Customer>(HttpMethod.Post, "/v1/customers", request, null, cancellationToken);
    }

    public Task<Page<Customer>> ListAsync(ListCustomerParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Page<Customer>>(HttpMethod.Get, "/v1/customers", null, parameters, cancellationToken);

    public Task<Customer> RetrieveAsync(string id, RetrieveCustomerParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Customer>(HttpMethod.Get, Path("/v1/customers/{0}", id), null, parameters, cancellationToken);

    public Task<Customer> UpdateAsync(string id, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<Customer>(HttpMethod.Post, Path("/v1/customers/{0}", id), request, null, cancellationToken);
    }

    public Task<CustomerPrepaidBalance> RetrievePrepaidBalanceAsync(string customerId, RetrieveCustomerPrepaidBalanceParams parameters, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        return Requestor.SendAsync<CustomerPrepaidBalance>(HttpMethod.Get, Path("/v1/customers/{0}/prepaid", customerId), null, parameters, cancellationToken);
    }

    public Task<Page<CustomerWalletMovement>> ListWalletMovementsAsync(string id, ListCustomerWalletMovementParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Page<CustomerWalletMovement>>(HttpMethod.Get, Path("/v1/customers/{0}/wallet/movements", id), null, parameters, cancellationToken);

    public Task<Page<CustomerAddress>> ListAddressesAsync(string customerId, ListCustomerAddressParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Page<CustomerAddress>>(HttpMethod.Get, Path("/v1/customers/{0}/addresses", customerId), null, parameters, cancellationToken);

    public Task<CustomerAddress> CreateAddressAsync(string customerId, CustomerAddressRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<CustomerAddress>(HttpMethod.Post, Path("/v1/customers/{0}/addresses", customerId), request, null, cancellationToken);
    }

    public Task<CustomerAddress> RetrieveAddressAsync(string customerId, string addressId, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<CustomerAddress>(HttpMethod.Get, Path("/v1/customers/{0}/addresses/{1}", customerId, addressId), null, null, cancellationToken);

    public Task<CustomerAddress> UpdateAddressAsync(string customerId, string addressId, CustomerAddressRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<CustomerAddress>(HttpMethod.Put, Path("/v1/customers/{0}/addresses/{1}", customerId, addressId), request, null, cancellationToken);
    }

    public Task DeleteAddressAsync(string customerId, string addressId, CancellationToken cancellationToken = default) =>
        Requestor.SendNoContentAsync(HttpMethod.Delete, Path("/v1/customers/{0}/addresses/{1}", customerId, addressId), null, cancellationToken);
}
