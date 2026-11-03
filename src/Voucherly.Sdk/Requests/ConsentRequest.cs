using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Requests;

/// <summary>
/// One choice of the Customer as you collected it. IP address and user agent are the Customer's, not your server's.
/// </summary>
public class ConsentRequest : VoucherlyObject
{
    /// <summary>
    /// What the consent is for. <c>Marketing</c> is receiving promotional messages on a channel; <c>Profiling</c> is the analysis of the purchase history to tailor offers.
    /// </summary>
    /// <remarks>One of the <see cref="ConsentPurpose"/> values.</remarks>
    public string? Purpose { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Required for a marketing consent, and not allowed for profiling.
    /// </summary>
    /// <remarks>One of the <see cref="ConsentChannel"/> values.</remarks>
    public string? Channel { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// True when the Customer gives the consent, false when they refuse or revoke it.
    /// </summary>
    public bool Granted { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// When the Customer made the choice. It cannot be in the future. A choice older than the current one for the same purpose and channel is ignored. A consent collected more than 24 hours ago is recorded but starts no Marketing automation.
    /// </summary>
    public DateTimeOffset CollectedAt { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Where the choice was collected. Defaults to <c>Other</c>.
    /// </summary>
    /// <remarks>One of the <see cref="ConsentSource"/> values.</remarks>
    public string? Source { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The text the Customer was shown. Required when <c>granted</c> is true.
    /// </summary>
    public string? Text { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The version of that text in your own system.
    /// </summary>
    public string? Version { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The IP address of the Customer when they made the choice.
    /// </summary>
    public string? IpAddress { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The user agent of the Customer's browser when they made the choice.
    /// </summary>
    public string? UserAgent { get; set { field = value; MarkAssigned(); } }
}
