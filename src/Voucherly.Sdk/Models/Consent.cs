using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Models;

/// <summary>
/// The current choice of a Customer for one purpose, and for marketing one channel.
/// </summary>
public class Consent : VoucherlyObject
{
    /// <summary>
    /// What the consent is for. <c>Marketing</c> is receiving promotional messages; <c>Profiling</c> is the analysis of the purchase history to tailor offers.
    /// </summary>
    /// <remarks>One of the <see cref="ConsentPurpose"/> values.</remarks>
    public string? Purpose { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The channel of a marketing consent. Absent for profiling, which has no channel.
    /// </summary>
    /// <remarks>One of the <see cref="ConsentChannel"/> values.</remarks>
    public string? Channel { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the consent is given. False means the Customer revoked it, which for email is also the opposition to promotional email sent to past buyers.
    /// </summary>
    public bool Granted { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// When the Customer made this choice, in UTC.
    /// </summary>
    public DateTimeOffset CollectedAt { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The version of the consent text the Customer was shown.
    /// </summary>
    public string? Version { get; set { field = value; MarkAssigned(); } }
}
