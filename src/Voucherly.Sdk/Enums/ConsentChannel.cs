namespace Voucherly.Sdk.Enums;

/// <summary>
/// The channel of a marketing consent. Absent for profiling, which has no channel.
/// </summary>
public static class ConsentChannel
{
    public const string Email = "Email";

    public const string Sms = "Sms";

    public const string Whatsapp = "Whatsapp";
}
