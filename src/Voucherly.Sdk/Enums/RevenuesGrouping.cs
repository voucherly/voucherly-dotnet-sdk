namespace Voucherly.Sdk.Enums;

/// <summary>
/// A grouping dimension for volumes report rows. <c>Store</c> breaks the rows down per Store; <c>Company</c> breaks them down per Company. With no dimension at all, rows are aggregated per PaymentGateway only.
/// </summary>
public static class RevenuesGrouping
{
    public const string Store = "Store";

    public const string Company = "Company";
}
