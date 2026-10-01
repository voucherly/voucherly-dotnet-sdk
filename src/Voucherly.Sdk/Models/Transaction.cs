using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// A Transaction represents a single payment attempt or operation within a Payment. A Payment can have multiple Transactions.
/// </summary>
public class Transaction : VoucherlyObject
{
    /// <summary>
    /// Unique identifier for the transaction.
    /// </summary>
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the PaymentGatewayAccount that processed this transaction.
    /// </summary>
    public string? PaymentGatewayAccountId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the PaymentGateway used for this transaction.
    /// </summary>
    public string? PaymentGatewayId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Essential information about the PaymentGateway used for this transaction.
    /// </summary>
    public PaymentGatewayEssential? PaymentGateway { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the secondary PaymentGateway, when the transaction rides on another gateway (e.g. a wallet on a card gateway).
    /// </summary>
    public string? PaymentGatewayId2 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Terminal that took the payment, for card-present transactions.
    /// </summary>
    public string? TerminalId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The primary external transaction ID from the payment gateway provider.
    /// </summary>
    public string? ExternalId1 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// An additional external transaction ID or reference from the payment gateway provider.
    /// </summary>
    public string? ExternalId2 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A third external transaction ID or reference from the payment gateway provider.
    /// </summary>
    public string? ExternalId3 { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Error information if the transaction failed.
    /// </summary>
    public TransactionError? Error { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The date and time the transaction was requested, in UTC.
    /// </summary>
    public DateTimeOffset RequestedAt { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount that was requested for this transaction, in cents.
    /// </summary>
    public int RequestedAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The part of the requested amount the gateway could not charge because it sits below its minimum, in cents.
    /// </summary>
    public int ExceedingAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The actual amount processed in this transaction, in cents.
    /// </summary>
    public int Amount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount paid with a digital payment method in this transaction, in cents.
    /// </summary>
    public int DigitalAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount paid using vouchers in this transaction, in cents.
    /// </summary>
    public int VoucherAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount paid with a fringe benefit credit in this transaction, in cents.
    /// </summary>
    public int FringeAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount paid in cash in this transaction, in cents.
    /// </summary>
    public int CashAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The number of vouchers used in this transaction.
    /// </summary>
    public int? NoOfVouchers { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount that has been confirmed (captured) from this transaction, in cents.
    /// </summary>
    public int ConfirmedAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount that has been refunded from this transaction, in cents.
    /// </summary>
    public int RefundedAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The date and time the transaction was paid, in UTC.
    /// </summary>
    public DateTimeOffset? PaidOnUtc { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The currency code for this transaction (e.g., "EUR", "USD").
    /// </summary>
    public string? Currency { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Credit card information if this transaction was paid with a card.
    /// </summary>
    public CreditCardInfo? CreditCard { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Direct debit (SEPA) information if this transaction was paid via direct debit.
    /// </summary>
    public DirectDebitInfo? DirectDebit { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The email address of the payment method holder for this transaction.
    /// </summary>
    public string? HolderEmail { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The name of the payment method holder for this transaction.
    /// </summary>
    public string? HolderName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The current status of this transaction.
    /// </summary>
    /// <remarks>One of the <see cref="TransactionStatus"/> values.</remarks>
    public string? Status { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Information about the payment request, including any redirect URL or action required.
    /// </summary>
    public CheckoutPaymentRequest? Request { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The action the payer still has to perform for the transaction to complete.
    /// </summary>
    public TransactionNextAction? NextAction { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The confirm, refund, cancel and reverse operations performed on this transaction.
    /// </summary>
    public List<TransactionOperation>? Operations { get; set { field = value; MarkAssigned(); } }
}
