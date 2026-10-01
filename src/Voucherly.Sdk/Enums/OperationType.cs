namespace Voucherly.Sdk.Enums;

/// <summary>
/// The kind of operation performed on a Transaction.
/// </summary>
public static class OperationType
{
    public const string Confirm = "Confirm";

    public const string Refund = "Refund";

    public const string Cancel = "Cancel";

    public const string Reverse = "Reverse";
}
