namespace Voucherly.Sdk.Requests;

/// <summary>
/// Request body for updating a Company. Only the fields passed change.
/// </summary>
public class UpdateCompanyRequest : VoucherlyObject
{
    /// <summary>
    /// Company name.
    /// </summary>
    public string Name { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// A unique 6-character code that can be used to join or reference this Company. Another Company with the same code answers 409.
    /// </summary>
    public string JoinCode { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The ID of the Company in your own system. Null removes it. Another Company with the same reference answers 409.
    /// </summary>
    public string? ReferenceId { get; set { field = value; MarkAssigned(); } }
}
