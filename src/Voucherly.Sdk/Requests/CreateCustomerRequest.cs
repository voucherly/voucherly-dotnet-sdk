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
    /// The ID of the customer in your own system, such as the user ID of your website. It is unique per environment, so two Customers of the same environment can't share it. When another Customer already has it, the request fails with <c>409 Conflict</c>.
    /// </summary>
    public string? ReferenceId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The language to write to the Customer in, as an ISO 639-1 two-letter code such as <c>it</c> or <c>en</c>.
    /// </summary>
    public string? Language { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Company this Customer is associated with.
    /// </summary>
    public string? CompanyId { get; set { field = value; MarkAssigned(); } }

    public Dictionary<string, string>? Metadata { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The choices the Customer made about marketing and profiling, as you collected them. Each one is kept in the consent register with its date, text and version.
    /// </summary>
    public List<ConsentRequest>? Consents { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Optional settings for customer creation.
    /// </summary>
    public CreateCustomerRequestOptions? Options { get; set { field = value; MarkAssigned(); } }
}
