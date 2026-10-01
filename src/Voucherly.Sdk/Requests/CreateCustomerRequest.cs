namespace Voucherly.Sdk.Requests;

/// <summary>
/// Request body for creating a Customer.
/// </summary>
public class CreateCustomerRequest : VoucherlyObject
{
    /// <summary>
    /// The customer’s email address.
    /// </summary>
    public string Email { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The customer’s first name.
    /// </summary>
    public string? FirstName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The customer’s last name.
    /// </summary>
    public string? LastName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The customer’s phone number.
    /// </summary>
    public string? PhoneNumber { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Company this Customer is associated with.
    /// </summary>
    public string? CompanyId { get; set { field = value; MarkAssigned(); } }

    public Dictionary<string, string>? Metadata { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Optional settings for customer creation.
    /// </summary>
    public CreateCustomerRequestOptions? Options { get; set { field = value; MarkAssigned(); } }
}
