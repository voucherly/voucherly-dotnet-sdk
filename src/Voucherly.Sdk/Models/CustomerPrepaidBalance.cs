namespace Voucherly.Sdk.Models;

/// <summary>
/// The prepaid balance available to a Customer for a given date.
/// </summary>
public class CustomerPrepaidBalance : VoucherlyObject
{
    /// <summary>
    /// Indicates whether the prepaid PaymentGateway is currently configured and active for the Merchant.
    /// </summary>
    public bool PaymentGatewayIsActive { get; set { field = value; MarkAssigned(); } }

    public PrepaidPolicy? Policy { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The remaining amount, in cents, that the Customer is still allowed to spend on the requested Date.
    /// </summary>
    public int? AvailableAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The amount, in cents, already spent by the Customer on the requested Date.
    /// </summary>
    public int UsedAmount { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The Date the balance refers to.
    /// </summary>
    public DateOnly Date { get; set { field = value; MarkAssigned(); } }
}
