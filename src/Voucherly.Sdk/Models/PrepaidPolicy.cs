namespace Voucherly.Sdk.Models;

/// <summary>
/// The prepaid policy that the Customer is subject to.
/// </summary>
public class PrepaidPolicy : VoucherlyObject
{
    /// <summary>
    /// Indicates whether the prepaid policy is currently active.
    /// </summary>
    public bool IsActive { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The maximum amount, in cents, that the Customer is allowed to spend in a single Date under this policy.
    /// </summary>
    public int? Amount { get; set { field = value; MarkAssigned(); } }
}
