namespace Voucherly.Sdk.Requests;

/// <summary>
/// Request body for updating a Customer. Any field not provided will be left unchanged. <c>referenceId</c> cannot be changed via this endpoint.
/// </summary>
public class UpdateCustomerRequest : VoucherlyObject
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
    /// The ID of the Company this Customer is associated with. Pass <c>null</c> to detach the Customer from its Company.
    /// </summary>
    public string? CompanyId { get; set { field = value; MarkAssigned(); } }

    public Dictionary<string, string>? Metadata { get; set { field = value; MarkAssigned(); } }
}
