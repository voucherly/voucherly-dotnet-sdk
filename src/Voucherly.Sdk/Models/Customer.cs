using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// The Customer object.
/// </summary>
public class Customer : VoucherlyObject
{
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the merchant this Customer belongs to, that is your own merchant.
    /// </summary>
    public string? MerchantId { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="TenantMode"/> values.</remarks>
    public string? Tenant { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The date and time the Customer was created, in UTC.
    /// </summary>
    public DateTimeOffset Created { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The date and time the Customer registered on your website, in UTC, as you declared it with <c>registeredAt</c>. Null for the Customers not created through this API.
    /// </summary>
    public DateTimeOffset? Registered { get; set { field = value; MarkAssigned(); } }

    public Origin? Origin { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The customer’s email address.
    /// </summary>
    public string? Email { get; set { field = value; MarkAssigned(); } }

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
    /// The ID of the customer in your own system, such as the user ID of your website. It is unique per environment, so two Customers of the same environment can't share it.
    /// </summary>
    public string? ReferenceId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The language the Customer is written to in, as an ISO 639-1 two-letter code.
    /// </summary>
    public string? Language { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The Customer's date of birth, as <c>yyyy-MM-dd</c>.
    /// </summary>
    public DateOnly? BirthDate { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Company this Customer is associated with.
    /// </summary>
    public string? CompanyId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The Company this Customer is associated with, returned in full when one exists.
    /// </summary>
    public Company? Company { get; set { field = value; MarkAssigned(); } }

    public Packaging? Packaging { get; set { field = value; MarkAssigned(); } }

    public CustomerWalletTotals? Wallet { get; set { field = value; MarkAssigned(); } }

    public SpendData? SpendData { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The current choice of the Customer for each purpose and channel it expressed one for. A consent never expressed is absent.
    /// </summary>
    public List<Consent>? Consents { get; set { field = value; MarkAssigned(); } }

    public Dictionary<string, string>? Metadata { get; set { field = value; MarkAssigned(); } }
}
