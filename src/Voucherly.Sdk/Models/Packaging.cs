using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// The packaging set on the Customer itself, for merchants that operate a reusable container scheme. Absent when the Customer inherits both the type and the deposit.
/// </summary>
public class Packaging : VoucherlyObject
{
    /// <summary>
    /// Whether the Customer is on reusable or disposable packaging. Null when the Customer inherits the type from its Company, or from the ecommerce site default.
    /// </summary>
    /// <remarks>One of the <see cref="PackagingType"/> values.</remarks>
    public string? Type { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the deposit on the reusable packaging has been paid.
    /// </summary>
    public bool? IsDepositPaid { get; set { field = value; MarkAssigned(); } }
}
