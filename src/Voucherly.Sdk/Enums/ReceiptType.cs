namespace Voucherly.Sdk.Enums;

/// <summary>
/// The fiscal type of the Receipt:
/// - <c>Sale</c>: receipt issued for a successful Payment.
/// - <c>Void</c>: receipt issued for a void or refund operation.
/// </summary>
public static class ReceiptType
{
    public const string Sale = "Sale";

    public const string Void = "Void";
}
