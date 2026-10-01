using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// The error encountered during the Transaction, if any.
/// </summary>
public class TransactionError : VoucherlyObject
{
    /// <remarks>One of the <see cref="TransactionErrorCode"/> values.</remarks>
    public string? Code { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="DeclineErrorCode"/> values.</remarks>
    public string? DeclineErrorCode { get; set { field = value; MarkAssigned(); } }

    public ExternalError? ExternalError { get; set { field = value; MarkAssigned(); } }
}
