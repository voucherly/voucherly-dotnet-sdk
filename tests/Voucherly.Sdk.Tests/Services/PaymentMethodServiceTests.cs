using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Services;

public class PaymentMethodServiceTests : ServiceTestBase
{
    [Fact]
    public async Task List()
    {
        var json = RespondWithSample("list-customer-payment-method");

        var page = await Client.PaymentMethods.ListAsync("cs_YZOJp96qKlW", new ListCustomerPaymentMethodParams { Length = 100, Start = "pm_next" });

        AssertOperation("list-customer-payment-method", new Dictionary<string, string> { ["customerId"] = "cs_YZOJp96qKlW" }, "length=100&start=pm_next");
        page.Items[0].ShouldBeOfType<PaymentMethod>();
        AssertReadsEveryMember(json, page);
    }

    [Fact]
    public async Task Delete()
    {
        RespondWithSample("delete-customer-payment-method");

        await Client.PaymentMethods.DeleteAsync("cs_YZOJp96qKlW", "pm_1");

        AssertOperation("delete-customer-payment-method", new Dictionary<string, string> { ["customerId"] = "cs_YZOJp96qKlW", ["paymentMethodId"] = "pm_1" });
    }
}
