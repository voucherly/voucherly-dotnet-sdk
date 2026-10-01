namespace Voucherly.Sdk.Requests;

/// <summary>
/// Optional settings for customer creation.
/// </summary>
public class CreateCustomerRequestOptions : VoucherlyObject
{
    /// <summary>
    /// If true, the API will return an error if a customer with the same email address already exists. If false, allows duplicate emails.
    /// </summary>
    public bool RequireUniqueEmail { get; set { field = value; MarkAssigned(); } }
}
