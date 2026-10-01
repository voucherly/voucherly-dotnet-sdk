using System.Collections;
using Voucherly.Sdk.Enums;
using Voucherly.Sdk.Exceptions;
using Voucherly.Sdk.Requests;
using Xunit.Abstractions;

namespace Voucherly.Sdk.Tests.Live;

/// <summary>
/// Read-only calls against the sandbox, run only when VOUCHERLY_API_KEY holds a sandbox key.
/// </summary>
[Trait("Category", "Live")]
public class SandboxReadTests(ITestOutputHelper output)
{
    private readonly VoucherlyClient _client = new(new VoucherlyClientOptions { ApiKey = LiveFactAttribute.ApiKey ?? "unused", App = "voucherly-dotnet-sdk-tests" });

    [LiveFact]
    public async Task ListsPaymentGateways()
    {
        var list = await _client.PaymentGateways.ListAsync();

        list.Items.ShouldNotBeNull().Count.ShouldBeGreaterThan(0);
        ReportUnknownMembers(list);
    }

    [LiveFact]
    public async Task ListsAndRetrievesCompanies()
    {
        var page = await _client.Companies.ListAsync();

        page.Pagination.ShouldNotBeNull();
        ReportUnknownMembers(page);
        if (page.Items.Count > 0)
        {
            ReportUnknownMembers(await _client.Companies.RetrieveAsync(page.Items[0].Id!));
        }
    }

    [LiveFact]
    public async Task ReadsTheCustomersAndWhatHangsFromThem()
    {
        var page = await _client.Customers.ListAsync(new ListCustomerParams { Length = 5 });
        ReportUnknownMembers(page);
        if (page.Pagination?.HasMore == true)
        {
            ReportUnknownMembers(await _client.Customers.ListAsync(new ListCustomerParams { Length = 5, Start = page.Pagination.NextStart }));
        }

        if (page.Items.Count == 0)
        {
            output.WriteLine("The sandbox has no Customer to read.");
            return;
        }

        var customerId = page.Items[0].Id!;
        ReportUnknownMembers(await _client.Customers.RetrieveAsync(customerId, new RetrieveCustomerParams { Include = [CustomerInclude.Wallet] }));
        ReportUnknownMembers(await _client.Customers.ListAddressesAsync(customerId));
        ReportUnknownMembers(await _client.Customers.ListWalletMovementsAsync(customerId));
        ReportUnknownMembers(await _client.PaymentMethods.ListAsync(customerId));
        ReportUnknownMembers(await _client.Customers.RetrievePrepaidBalanceAsync(customerId, new RetrieveCustomerPrepaidBalanceParams { Date = DateOnly.FromDateTime(DateTime.Today) }));
    }

    [LiveFact]
    public async Task ReadsTheStores()
    {
        var page = await _client.Stores.ListAsync(new ListStoreParams { Include = [StoreInclude.ConceptStore, StoreInclude.StoreArea, StoreInclude.Status] });
        ReportUnknownMembers(page);
        if (page.Items.Count > 0)
        {
            ReportUnknownMembers(await _client.Stores.RetrieveAsync(page.Items[0].Id!));
        }

        ReportUnknownMembers(await _client.ConceptStores.ListAsync());
        ReportUnknownMembers(await _client.StoreAreas.ListAsync());
        ReportUnknownMembers(await _client.Terminals.ListAsync());
    }

    [LiveFact]
    public async Task ReadsTheVolumesReport()
    {
        var report = await _client.Reports.VolumesAsync(new VolumesReportParams { FromDate = DateTimeOffset.UtcNow.AddDays(-30), ToDate = DateTimeOffset.UtcNow });

        report.Totals.ShouldNotBeNull();
        ReportUnknownMembers(report);
    }

    [LiveFact]
    public async Task AMalformedIdIsRejectedWithTheParameterInError()
    {
        var exception = await Should.ThrowAsync<BadRequestException>(() => _client.Payments.RetrieveAsync("pay_not_an_id"));

        exception.ErrorCode.ShouldBe("VALIDATION_ERROR");
        exception.Parameter.ShouldNotBeNull();
    }

    [LiveFact]
    public async Task AWrongKeyIsRejected()
    {
        var client = new VoucherlyClient(new VoucherlyClientOptions { ApiKey = "sk_sand_not_a_key" });

        var exception = await Should.ThrowAsync<ApiException>(() => client.PaymentGateways.ListAsync());

        exception.StatusCode.ShouldBe(401);
    }

    private void ReportUnknownMembers(object? value, string path = "")
    {
        switch (value)
        {
            case VoucherlyObject voucherlyObject:
                foreach (var member in voucherlyObject.ExtensionData?.Keys ?? [])
                {
                    output.WriteLine($"Member the spec does not document: {value.GetType().Name}.{member}");
                }

                foreach (var property in value.GetType().GetProperties().Where(property => property.Name != nameof(VoucherlyObject.ExtensionData)))
                {
                    ReportUnknownMembers(property.GetValue(value), $"{path}.{property.Name}");
                }

                break;
            case IEnumerable items and not string and not IDictionary:
                foreach (var item in items)
                {
                    ReportUnknownMembers(item, path);
                }

                break;
        }
    }
}

public sealed class LiveFactAttribute : FactAttribute
{
    public static readonly string? ApiKey = Environment.GetEnvironmentVariable("VOUCHERLY_API_KEY");

    public LiveFactAttribute()
    {
        if (string.IsNullOrEmpty(ApiKey))
        {
            Skip = "VOUCHERLY_API_KEY is not set.";
        }
        else if (!ApiKey.StartsWith("sk_sand_", StringComparison.Ordinal))
        {
            Skip = "The live tests run only with a sandbox key (sk_sand_).";
        }
    }
}
