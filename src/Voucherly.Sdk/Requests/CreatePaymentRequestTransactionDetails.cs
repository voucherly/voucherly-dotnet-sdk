namespace Voucherly.Sdk.Requests;

/// <summary>
/// Gateway-specific details of the transaction. Only <c>POS</c> takes them today, to say which card reader must collect the payment: give exactly one of the three identifiers below. The Terminal is looked up across the whole merchant, so it must be paired and, for <c>referenceTerminalId</c>, carry that reference — otherwise the request fails with <c>RESOURCE_MISSING</c> and no Payment is created.
/// </summary>
public class CreatePaymentRequestTransactionDetails : VoucherlyObject
{
    /// <summary>
    /// The ID of the Terminal in Voucherly, in the form <c>term_01kg2gestgepdbmsn7hs6bsrwp</c>.
    /// </summary>
    public string? TerminalId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The ID the payment gateway gave the reader, as returned by the Terminals endpoint.
    /// </summary>
    public string? ExternalTerminalId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Your own reference for the reader, set on the Terminal from the Voucherly dashboard. Use it to address a reader by the code your cash register already knows, such as <c>00011.007</c>.
    /// </summary>
    public string? ReferenceTerminalId { get; set { field = value; MarkAssigned(); } }
}
