namespace Voucherly.Sdk.Requests;

/// <summary>
/// Request body for creating a Company.
/// </summary>
public class CreateCompanyRequest : VoucherlyObject
{
    /// <summary>
    /// A unique 6-character code that can be used to join or reference this Company.
    /// </summary>
    public string JoinCode { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// Company name.
    /// </summary>
    public string Name { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The ID of the Company in your own system. Another Company with the same reference answers 409.
    /// </summary>
    public string? ReferenceId { get; set { field = value; MarkAssigned(); } }
}
