using Voucherly.Sdk.Enums;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Services;

public class CompanyServiceTests : ServiceTestBase
{
    [Fact]
    public async Task Create()
    {
        var json = RespondWithSample("create-company");

        var company = await Client.Companies.CreateAsync(new CreateCompanyRequest { Name = "Acme S.p.A.", JoinCode = "ACME01" });

        AssertOperation("create-company", body: new System.Text.Json.Nodes.JsonObject { ["name"] = "Acme S.p.A.", ["joinCode"] = "ACME01" });
        AssertReadsEveryMember(json, company);
    }

    [Fact]
    public async Task List()
    {
        var json = RespondWithSample("list-company");

        var page = await Client.Companies.ListAsync(new ListCompanyParams { Length = 20, Start = "cmp_next" });

        AssertOperation("list-company", query: "length=20&start=cmp_next");
        page.Items[0].ShouldBeOfType<Company>();
        AssertReadsEveryMember(json, page);
    }

    [Fact]
    public async Task ListWithoutParameters()
    {
        RespondWithSample("list-company");

        await Client.Companies.ListAsync();

        AssertOperation("list-company");
    }

    [Fact]
    public async Task Retrieve()
    {
        var json = RespondWithSample("retrieve-company");

        var company = await Client.Companies.RetrieveAsync("ACME01", new RetrieveCompanyParams { Include = [CompanyInclude.Addresses] });

        AssertOperation("retrieve-company", new Dictionary<string, string> { ["idOrCode"] = "ACME01" }, "include=Addresses");
        AssertReadsEveryMember(json, company);
    }
}
