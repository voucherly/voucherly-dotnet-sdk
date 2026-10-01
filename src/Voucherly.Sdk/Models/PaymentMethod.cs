namespace Voucherly.Sdk.Models;

/// <summary>
/// The PaymentMethod object.
/// </summary>
public class PaymentMethod : VoucherlyObject
{
    public string? Id { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the Customer to which this PaymentMethod is saved.
    /// </summary>
    public string? CustomerId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the PaymentGatewayAccount that holds this PaymentMethod.
    /// </summary>
    public string? PaymentGatewayAccountId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the PaymentGateway through which this PaymentMethod is processed.
    /// </summary>
    public string? PaymentGatewayId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID of the PaymentMethod as registered in the external PaymentGateway system. Used to reference the method in operations with the provider.
    /// </summary>
    public string? ExternalId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Email address associated with the PaymentMethod holder. This may differ from the Customer's email.
    /// </summary>
    public string? HolderEmail { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Full name of the individual or entity that owns the PaymentMethod. This may differ from the Customer's registered name.
    /// </summary>
    public string? HolderName { get; set { field = value; MarkAssigned(); } }

    public CreditCardInfo? CreditCard { get; set { field = value; MarkAssigned(); } }

    public DirectDebitInfo? DirectDebit { get; set { field = value; MarkAssigned(); } }
}
