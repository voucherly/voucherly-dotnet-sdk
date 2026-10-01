namespace Voucherly.Sdk.Models;

/// <summary>
/// If this is a sepa_debit PaymentMethod, this contains the user’s card details.
/// </summary>
public class DirectDebitInfo : VoucherlyObject
{
    /// <summary>
    /// ID of the bank account.
    /// </summary>
    public string? AccountId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// BIC of the bank account.
    /// </summary>
    public string? AccountBic { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// IBAN of the bank account.
    /// </summary>
    public string? AccountIban { get; set { field = value; MarkAssigned(); } }
}
