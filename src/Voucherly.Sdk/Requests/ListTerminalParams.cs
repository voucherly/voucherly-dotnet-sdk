using Voucherly.Sdk.Enums;

namespace Voucherly.Sdk.Requests;

public sealed class ListTerminalParams : RequestParams
{
    /// <summary>
    /// Filter by the PaymentGatewayAccount the Terminal belongs to.
    /// </summary>
    public string? PaymentGatewayAccountId { get; set; }

    /// <summary>
    /// Filter by the PaymentGateway the Terminal belongs to.
    /// </summary>
    public string? PaymentGatewayId { get; set; }

    /// <summary>
    /// Filter by the Store the Terminal is bound to.
    /// </summary>
    public string? StoreId { get; set; }

    /// <summary>
    /// Filter by Terminal status. When omitted, deleted Terminals are excluded.
    /// </summary>
    /// <remarks>One of the <see cref="TerminalStatus"/> values.</remarks>
    public string? Status { get; set; }

    /// <summary>
    /// A limit on the number of objects to be returned. Limit can range between 1 and 100, and the default is 10.
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// A cursor for pagination across multiple pages of results. Don’t include this parameter on the first call. Use the <c>nextStart</c> value returned in a previous response to request subsequent results.
    /// </summary>
    public string? Start { get; set; }
}
