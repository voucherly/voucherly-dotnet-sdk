namespace Voucherly.Sdk.Requests;

/// <summary>
/// Request body for updating a Customer. Any field not provided will be left unchanged.
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
    /// The ID of the customer in your own system, such as the user ID of your website. It is unique per environment, so two Customers of the same environment can't share it. Use it to link a Customer that already exists to your user, or pass <c>null</c> to unlink it. When another Customer already has it, the request fails with <c>409 Conflict</c>.
    /// </summary>
    public string? ReferenceId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The language to write to the Customer in, as an ISO 639-1 two-letter code such as <c>it</c> or <c>en</c>.
    /// </summary>
    public string? Language { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Company this Customer is associated with. Pass <c>null</c> to detach the Customer from its Company.
    /// </summary>
    public string? CompanyId { get; set { field = value; MarkAssigned(); } }

    public Dictionary<string, string>? Metadata { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Choices the Customer made after the last update, appended to the consent register. They never replace it, and a choice equal to the current one, with the same version, is ignored, so you can send the whole list at every update.
    /// </summary>
    public List<ConsentRequest>? Consents { get; set { field = value; MarkAssigned(); } }
}
