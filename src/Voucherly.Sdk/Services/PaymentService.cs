using Voucherly.Sdk.Http;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk.Services;

public interface IPaymentService
{
    /// <summary>
    /// Create a Payment.
    /// </summary>
    Task<Payment> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a Payment.
    /// </summary>
    Task<Payment> RetrieveAsync(string id, RetrievePaymentParams? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirm a Payment.
    /// Captures a <c>Paid</c> Payment.
    /// Send the lines actually delivered, or the final and food amounts, and Voucherly decides how much to capture, void or give back on each transaction.
    /// The Payment stays <c>Paid</c> until every operation has succeeded.
    /// When a payment gateway fails midway, the response lists the operations already done and the same request can be sent again: it settles only what is left.
    /// </summary>
    Task<Payment> ConfirmAsync(string id, ConfirmPaymentRequest? request = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refund a Payment.
    /// </summary>
    Task<Payment> RefundAsync(string id, RefundPaymentRequest? request = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Void a Payment.
    /// Voids a Payment that has not yet been paid.
    /// Only Payments in <c>Requested</c> status can be voided.
    /// </summary>
    Task<Payment> VoidAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download a Payment receipt.
    /// Downloads the fiscal sale receipt issued for a Payment, as a PDF document.
    /// </summary>
    Task<byte[]> DownloadReceiptAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download a Payment refund receipt.
    /// Downloads the fiscal void/refund receipt issued for a Payment, as a PDF document.
    /// </summary>
    Task<byte[]> DownloadRefundReceiptAsync(string id, CancellationToken cancellationToken = default);
}

internal sealed class PaymentService(ApiRequestor requestor) : ServiceBase(requestor), IPaymentService
{
    public Task<Payment> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Requestor.SendAsync<Payment>(HttpMethod.Post, "/v1/payments", request, null, cancellationToken);
    }

    public Task<Payment> RetrieveAsync(string id, RetrievePaymentParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Payment>(HttpMethod.Get, Path("/v1/payments/{0}", id), null, parameters, cancellationToken);

    public Task<Payment> ConfirmAsync(string id, ConfirmPaymentRequest? request = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Payment>(HttpMethod.Post, Path("/v1/payments/{0}/confirm", id), request, null, cancellationToken);

    public Task<Payment> RefundAsync(string id, RefundPaymentRequest? request = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Payment>(HttpMethod.Post, Path("/v1/payments/{0}/refund", id), request, null, cancellationToken);

    public Task<Payment> VoidAsync(string id, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<Payment>(HttpMethod.Post, Path("/v1/payments/{0}/void", id), null, null, cancellationToken);

    public Task<byte[]> DownloadReceiptAsync(string id, CancellationToken cancellationToken = default) =>
        Requestor.SendBytesAsync(Path("/v1/payments/{0}/receipt", id), "application/pdf", cancellationToken);

    public Task<byte[]> DownloadRefundReceiptAsync(string id, CancellationToken cancellationToken = default) =>
        Requestor.SendBytesAsync(Path("/v1/payments/{0}/refund_receipt", id), "application/pdf", cancellationToken);
}
