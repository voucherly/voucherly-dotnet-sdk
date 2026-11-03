namespace Voucherly.Sdk.Requests;

/// <summary>
/// Inline description of the customer of the Payment. The ID of an existing Customer is not part of it: send it as <c>customerId</c>, next to <c>customer</c>.
/// - With <c>customerId</c> alone, the Payment goes to that Customer.
/// - With <c>referenceId</c> alone, the Payment goes to the Customer with that reference, and a new Customer with that reference is created when there is none.
/// - With both <c>customerId</c> and <c>referenceId</c>, they must name the same Customer. A Customer with no reference takes <c>referenceId</c>, unless another Customer already has it, which fails with <c>409 Conflict</c>. A Customer with a different reference fails with <c>400 Bad Request</c> and code <c>CUSTOMER_REFERENCE_ID_MISMATCH</c>: change the reference by updating the Customer.
/// - With neither, a new Customer is created, unless <c>emailVerified</c> or <c>phoneVerified</c> find the Customer that already has that email or phone.
/// A verified email or phone binds the Payment to an existing Customer only when no Customer was found by <c>customerId</c> or <c>referenceId</c>. The email is tried first, then the phone. With a <c>referenceId</c> no Customer has yet, only a Customer without a reference is found, and it takes <c>referenceId</c>. The checkout of a Payment shows the saved payment methods and the credit of its Customer: declare a contact verified only when the customer proved it is theirs, for example by logging in with a confirmed email.
/// It replaces the deprecated <c>customerEmail</c>, <c>customerFirstName</c>, <c>customerLastName</c> and <c>customerPhoneNumber</c>: do not send them together with it.
/// When both are sent, whichever comes later in the body wins: <c>customer</c> replaces all of them, while each of them overwrites only its own field of <c>customer</c>.
/// </summary>
public class CreatePaymentRequestCustomer : VoucherlyObject
{
    /// <summary>
    /// The ID of the customer in your own system, such as the user ID of your website, unique per environment. It lets you charge the same Customer again without storing its Voucherly ID.
    /// </summary>
    public string? ReferenceId { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The email address of the customer associated with this Payment. Required when <c>customerId</c> is not provided.
    /// </summary>
    public string? Email { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The first name of the customer associated with this Payment.
    /// </summary>
    public string? FirstName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The last name of the customer associated with this Payment.
    /// </summary>
    public string? LastName { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// The phone number of the customer associated with this Payment.
    /// </summary>
    public string? PhoneNumber { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the customer proved that <c>email</c> is theirs. Requires <c>email</c>, or the request fails with code <c>CUSTOMER_EMAIL_REQUIRED</c>. Defaults to false.
    /// </summary>
    public bool EmailVerified { get; set { field = value; MarkAssigned(); } }

    /// <summary>
    /// Whether the customer proved that <c>phoneNumber</c> is theirs. Requires <c>phoneNumber</c>, or the request fails with code <c>CUSTOMER_PHONE_REQUIRED</c>. Defaults to false.
    /// </summary>
    public bool PhoneVerified { get; set { field = value; MarkAssigned(); } }
}
