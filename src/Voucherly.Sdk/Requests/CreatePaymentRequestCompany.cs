namespace Voucherly.Sdk.Requests;

/// <summary>
/// The Company of the customer, identified by your own company ID, so you never have to create it in advance or store its Voucherly ID.
/// The Payment looks for the Company with <c>referenceId</c>. When there is none, it creates one with <c>name</c> and <c>joinCode</c>. When there is one with a different <c>name</c>, it renames it: the name in your system wins.
/// The Customer of the Payment then joins the Company and stays in it, however it was found: a new Customer is created in it, and a Customer in another Company moves to it, as when a person changes employer in your system.
/// The same applies to <c>companyId</c>. You cannot specify both <c>company</c> and <c>companyId</c>.
/// </summary>
public class CreatePaymentRequestCompany : VoucherlyObject
{
    /// <summary>
    /// The ID of the Company in your own system, unique per environment.
    /// </summary>
    public string ReferenceId { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The name of the Company. It renames the Company when it differs.
    /// </summary>
    public string Name { get; set { field = value; MarkAssigned(); } } = null!;

    /// <summary>
    /// The 6-character code employees use to join the Company, used only when the Company is created. When it is missing, is not 6 characters long or another Company already has it, Voucherly generates one, so the Payment never fails because of it.
    /// </summary>
    public string? JoinCode { get; set { field = value; MarkAssigned(); } }
}
