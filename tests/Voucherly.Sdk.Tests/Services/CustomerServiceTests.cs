using System.Text.Json.Nodes;
using Voucherly.Sdk.Enums;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Services;

public class CustomerServiceTests : ServiceTestBase
{
    private const string CustomerId = "cs_YZOJp96qKlW";

    public static TheoryData<string> CreateExamples() => [.. SpecExamples.RequestExamples("create-customer").Keys];

    [Theory]
    [MemberData(nameof(CreateExamples))]
    public async Task Create(string example)
    {
        var json = RespondWithSample("create-customer");
        var body = SpecExamples.RequestExamples("create-customer")[example];

        var customer = await Client.Customers.CreateAsync(Deserialize<CreateCustomerRequest>(body));

        AssertOperation("create-customer", body: body);
        AssertReadsEveryMember(json, customer);
    }

    [Fact]
    public async Task List()
    {
        var json = RespondWithSample("list-customer");

        var page = await Client.Customers.ListAsync(new ListCustomerParams { Email = "mario.rossi@example.com", Length = 1 });

        AssertOperation("list-customer", query: "email=mario.rossi%40example.com&length=1");
        AssertReadsEveryMember(json, page);
    }

    [Fact]
    public async Task Retrieve()
    {
        var json = RespondWithSample("retrieve-customer");

        var customer = await Client.Customers.RetrieveAsync(CustomerId, new RetrieveCustomerParams { Include = [CustomerInclude.Wallet] });

        AssertOperation("retrieve-customer", new Dictionary<string, string> { ["id"] = CustomerId }, "include=Wallet");
        AssertReadsEveryMember(json, customer);
    }

    [Fact]
    public async Task UpdateSendsOnlyTheAssignedFieldsAndAnExplicitNull()
    {
        var json = RespondWithSample("update-customer");

        var customer = await Client.Customers.UpdateAsync(CustomerId, new UpdateCustomerRequest { Email = "mario.rossi@example.com", PhoneNumber = null, Metadata = new() { ["crmId"] = "42" } });

        AssertOperation("update-customer", new Dictionary<string, string> { ["id"] = CustomerId }, body: JsonNode.Parse("""{"email":"mario.rossi@example.com","phoneNumber":null,"metadata":{"crmId":"42"}}"""));
        AssertReadsEveryMember(json, customer);
    }

    [Fact]
    public async Task RetrievePrepaidBalance()
    {
        var json = RespondWithSample("retrieve-customer-prepaid-balance");

        var balance = await Client.Customers.RetrievePrepaidBalanceAsync(CustomerId, new RetrieveCustomerPrepaidBalanceParams { Date = new DateOnly(2026, 9, 30) });

        AssertOperation("retrieve-customer-prepaid-balance", new Dictionary<string, string> { ["customerId"] = CustomerId }, "date=2026-09-30");
        AssertReadsEveryMember(json, balance);
    }

    [Fact]
    public async Task ListWalletMovements()
    {
        var json = RespondWithSample("list-customer-wallet-movement");

        var page = await Client.Customers.ListWalletMovementsAsync(CustomerId, new ListCustomerWalletMovementParams
        {
            FromDate = new DateTimeOffset(2026, 9, 1, 0, 0, 0, TimeSpan.Zero),
            ToDate = new DateTimeOffset(2026, 9, 30, 23, 59, 59, TimeSpan.Zero),
        });

        AssertOperation("list-customer-wallet-movement", new Dictionary<string, string> { ["id"] = CustomerId }, "fromDate=2026-09-01T00%3A00%3A00%2B00%3A00&toDate=2026-09-30T23%3A59%3A59%2B00%3A00");
        page.Items[0].ShouldBeOfType<CustomerWalletMovement>();
        AssertReadsEveryMember(json, page);
    }

    [Fact]
    public async Task ListAddresses()
    {
        var json = RespondWithSample("list-customer-address");

        var page = await Client.Customers.ListAddressesAsync(CustomerId, new ListCustomerAddressParams { Length = 10, Start = "addr_next" });

        AssertOperation("list-customer-address", new Dictionary<string, string> { ["customerId"] = CustomerId }, "length=10&start=addr_next");
        page.Items[0].ShouldBeOfType<CustomerAddress>();
        AssertReadsEveryMember(json, page);
    }

    [Fact]
    public async Task CreateAddress()
    {
        var json = RespondWithSample("create-customer-address");

        var address = await Client.Customers.CreateAddressAsync(CustomerId, Deserialize<CustomerAddressRequest>(AddressJson()));

        AssertOperation("create-customer-address", new Dictionary<string, string> { ["customerId"] = CustomerId }, body: AddressJson());
        AssertReadsEveryMember(json, address);
    }

    [Fact]
    public async Task RetrieveAddress()
    {
        var json = RespondWithSample("retrieve-customer-address");

        var address = await Client.Customers.RetrieveAddressAsync(CustomerId, "addr_1");

        AssertOperation("retrieve-customer-address", new Dictionary<string, string> { ["customerId"] = CustomerId, ["addressId"] = "addr_1" });
        AssertReadsEveryMember(json, address);
    }

    [Fact]
    public async Task UpdateAddress()
    {
        var json = RespondWithSample("update-customer-address");

        var address = await Client.Customers.UpdateAddressAsync(CustomerId, "addr_1", Deserialize<CustomerAddressRequest>(AddressJson()));

        AssertOperation("update-customer-address", new Dictionary<string, string> { ["customerId"] = CustomerId, ["addressId"] = "addr_1" }, body: AddressJson());
        AssertReadsEveryMember(json, address);
    }

    [Fact]
    public async Task DeleteAddress()
    {
        RespondWithSample("delete-customer-address");

        await Client.Customers.DeleteAddressAsync(CustomerId, "addr_1");

        AssertOperation("delete-customer-address", new Dictionary<string, string> { ["customerId"] = CustomerId, ["addressId"] = "addr_1" });
    }

    private static JsonNode AddressJson() =>
        JsonNode.Parse("""{"label":"Home","streetName":"Via Roma","streetNumber":"1","city":"Milano","province":"MI","postalCode":"20121","country":"IT"}""")!;
}
