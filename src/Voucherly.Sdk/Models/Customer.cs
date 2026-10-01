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
    /// A unique identifier from your system that you can use to reference this Customer. This can be used to reconcile the Customer with your internal systems.
    /// </summary>
    public string? ReferenceId { get; set { field = value; MarkAssigned(); } }

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

    public Dictionary<string, string>? Metadata { get; set { field = value; MarkAssigned(); } }
}
