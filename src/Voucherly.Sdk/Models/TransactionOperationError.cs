namespace Voucherly.Sdk.Models;

/// <summary>
/// The error returned when an operation on a Transaction failed.
/// </summary>
public class TransactionOperationError : VoucherlyObject
{
    /// <summary>
    /// The error as reported by the PaymentGateway provider.
    /// </summary>
    public ExternalError? ExternalError { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the failure comes from Voucherly rather than from the PaymentGateway provider.
    /// </summary>
    public bool IsSystemError { get; set { field = value; MarkAssigned(); } }
}
