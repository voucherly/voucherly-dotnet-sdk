namespace Voucherly.Sdk.Requests;

public sealed class RetrieveCustomerPrepaidBalanceParams : RequestParams
{
    /// <summary>
    /// The date for which the prepaid balance must be evaluated, in <c>YYYY-MM-DD</c> format.
    /// </summary>
    public required DateOnly Date { get; set; }
}
