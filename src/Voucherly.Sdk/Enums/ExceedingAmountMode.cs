namespace Voucherly.Sdk.Enums;

/// <summary>
/// What to do when the amount to charge is below the minimum a payment gateway accepts — 0.50 EUR on most card gateways. The request is raised to that minimum, and this decides what happens to the part that exceeds what is due: with <c>None</c> it stays with you, with <c>Credit</c> it is credited to the customer's wallet. With <c>HideGateways</c> the amount is not raised at all: the gateway is not offered on the Checkout page, and a transaction requested on it fails with <c>AMOUNT_BELOW_MINIMUM</c>. Use <c>HideGateways</c> when nobody can pick another method, such as a charge on a terminal. If you don't specify anything the default Merchant configuration will be used.
/// </summary>
public static class ExceedingAmountMode
{
    public const string None = "None";

    public const string Credit = "Credit";

    public const string HideGateways = "HideGateways";
}
