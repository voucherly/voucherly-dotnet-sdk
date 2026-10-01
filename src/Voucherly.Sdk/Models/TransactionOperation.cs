using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// A single confirm, refund, cancel or reverse performed on a Transaction.
/// </summary>
public class TransactionOperation : VoucherlyObject
{
    /// <summary>
    /// The identifier of the operation.
    /// </summary>
    public int Id { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="OperationType"/> values.</remarks>
    public string? Operation { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount the operation moved, in cents.
    /// </summary>
    public int Amount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The date and time the operation was performed, in UTC.
    /// </summary>
    public DateTimeOffset OccurredOnUtc { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the operation succeeded.
    /// </summary>
    public bool Success { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The identifier of the operation in the PaymentGateway provider system.
    /// </summary>
    public string? ExternalId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The error returned when the operation failed.
    /// </summary>
    public TransactionOperationError? Error { get; set { field = value; MarkAssigned(); } }
}
