using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// The Company object.
/// </summary>
public class Company : VoucherlyObject
{
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the merchant this Company belongs to, that is your own merchant.
    /// </summary>
    public string? MerchantId { get; set { field = value; MarkAssigned(); } }

    /// <remarks>One of the <see cref="TenantMode"/> values.</remarks>
    public string? Tenant { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// A unique 6-character code that can be used to join or reference this Company.
    /// </summary>
    public string? JoinCode { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Company name.
    /// </summary>
    public string? Name { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The percentage applied to catalogue prices for this Company. Positive values raise them, negative values lower them.
    /// </summary>
    public int PriceAdjustmentPercentage { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The prepaid policy applied to the Customers of this Company.
    /// </summary>
    public PrepaidPolicy? PrepaidPolicy { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The packaging used by the Customers of this Company that do not set one of their own. Null when the Company follows the ecommerce site default.
    /// </summary>
    /// <remarks>One of the <see cref="PackagingType"/> values.</remarks>
    public string? PackagingType { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// An array of delivery addresses associated with this Company. Each address can have specific delivery fees and store information.
    /// </summary>
    public List<CompanyAddress>? Addresses { get; set { field = value; MarkAssigned(); } }
}
