using Voucherly.Sdk.Enums;
using Voucherly.Sdk.Exceptions;
using Voucherly.Sdk.Http;
using Voucherly.Sdk.Requests;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Live;

/// <summary>
/// Calls that create data in the sandbox, run only when VOUCHERLY_API_KEY holds a sandbox key and VOUCHERLY_LIVE_WRITES is 1.
/// </summary>
[Trait("Category", "Live")]
public class SandboxWriteTests
{
    private const string StoreName = "SDK test store";

    private readonly VoucherlyClient _client = new(new VoucherlyClientOptions { ApiKey = LiveWriteFactAttribute.ApiKey ?? "unused", App = "voucherly-dotnet-sdk-tests" });

    [LiveWriteFact]
    public async Task CustomerAndItsAddresses()
    {
        var email = $"sdk-dotnet-{Guid.NewGuid():N}@example.com";
        var customer = await _client.Customers.CreateAsync(new CreateCustomerRequest
        {
            Email = email,
            FirstName = "Mario",
            LastName = "Rossi",
            PhoneNumber = "+393331234567",
            Metadata = new() { ["source"] = "voucherly-dotnet-sdk-tests" },
        });

        (await _client.Customers.RetrieveAsync(customer.Id!)).Email.ShouldBe(email);

        var updated = await _client.Customers.UpdateAsync(customer.Id!, new UpdateCustomerRequest { FirstName = "Luigi", PhoneNumber = null });
        updated.FirstName.ShouldBe("Luigi");
        updated.LastName.ShouldBe("Rossi");
        updated.PhoneNumber.ShouldBeNull();

        var address = new CustomerAddressRequest { Label = "Home", StreetName = "Via Roma", StreetNumber = "1", City = "Milano", Province = "MI", PostalCode = "20121", Country = "IT" };
        var created = await _client.Customers.CreateAddressAsync(customer.Id!, address);
        address.Label = "Office";
        (await _client.Customers.UpdateAddressAsync(customer.Id!, created.Id!, address)).Label.ShouldBe("Office");
        (await _client.Customers.RetrieveAddressAsync(customer.Id!, created.Id!)).Label.ShouldBe("Office");

        var page = await _client.Customers.ListAddressesAsync(customer.Id!);
        page.Items.Select(item => item.Id).ShouldBe([created.Id]);

        await _client.Customers.DeleteAddressAsync(customer.Id!, created.Id!);
        await Should.ThrowAsync<NotFoundException>(() => _client.Customers.RetrieveAddressAsync(customer.Id!, created.Id!));
    }

    [LiveWriteFact]
    public async Task PaymentCreatedRetrievedAndVoided()
    {
        var request = System.Text.Json.JsonSerializer.Deserialize<CreatePaymentRequest>(SpecExamples.RequestExamples("create-payment")["New customer"].ToJsonString(), VoucherlyJson.Options)!;
        request.CustomerEmail = $"sdk-dotnet-{Guid.NewGuid():N}@example.com";
        request.ReferenceId = $"sdk-dotnet-{Guid.NewGuid():N}";

        var payment = await _client.Payments.CreateAsync(request);

        payment.Status.ShouldBe(PaymentStatus.Requested);
        payment.CheckoutUrl.ShouldNotBeNull();

        var retrieved = await _client.Payments.RetrieveAsync(payment.Id!, new RetrievePaymentParams { Include = [PaymentInclude.Lines, PaymentInclude.Discounts, PaymentInclude.Transactions] });
        retrieved.ReferenceId.ShouldBe(request.ReferenceId);
        retrieved.Lines.ShouldNotBeNull().Count.ShouldBe(1);
        retrieved.Lines[0].ProductName.ShouldBe("Muffin");

        (await _client.Payments.VoidAsync(payment.Id!)).Status.ShouldBe(PaymentStatus.Voided);

        var conflict = await Should.ThrowAsync<ConflictException>(() => _client.Payments.VoidAsync(payment.Id!));
        conflict.ErrorCode.ShouldNotBeNull();
    }

    [LiveWriteFact]
    public async Task ConceptStoreAndStoreArea()
    {
        var conceptStore = await _client.ConceptStores.CreateAsync(new ConceptStoreRequest { Name = $"SDK test {Guid.NewGuid():N}", ExternalId1 = "SDK-1" });
        (await _client.ConceptStores.UpdateAsync(conceptStore.Id!, new ConceptStoreRequest { Name = conceptStore.Name!, ExternalId1 = "SDK-2" })).ExternalId1.ShouldBe("SDK-2");
        (await _client.ConceptStores.ListAsync(new ListConceptStoreParams { Ids = [conceptStore.Id!] })).Items.Count.ShouldBe(1);
        await _client.ConceptStores.DeleteAsync(conceptStore.Id!);

        var storeArea = await _client.StoreAreas.CreateAsync(new StoreAreaRequest { Name = $"SDK test {Guid.NewGuid():N}" });
        (await _client.StoreAreas.RetrieveAsync(storeArea.Id!)).Name.ShouldBe(storeArea.Name);
        (await _client.StoreAreas.UpdateAsync(storeArea.Id!, new StoreAreaRequest { Name = storeArea.Name + " renamed" })).Name.ShouldEndWith(" renamed", Case.Sensitive);
        (await _client.StoreAreas.ListAsync(new ListStoreAreaParams { Ids = [storeArea.Id!] })).Items.Count.ShouldBe(1);
        await _client.StoreAreas.DeleteAsync(storeArea.Id!);

        await Should.ThrowAsync<NotFoundException>(() => _client.StoreAreas.RetrieveAsync(storeArea.Id!));
    }

    [LiveWriteFact]
    public async Task StoreCreatedOnceAndUpdated()
    {
        var existing = (await _client.Stores.ListAsync(new ListStoreParams { Name = StoreName })).Items;
        var request = System.Text.Json.JsonSerializer.Deserialize<StoreRequest>(SpecExamples.RequestExamples("create-store")["default"].ToJsonString(), VoucherlyJson.Options)!;
        request.Name = StoreName;
        request.Slug = "sdk-test-store";
        request.ExternalId1 = $"SDK-{Guid.NewGuid():N}"[..20];

        var store = existing.Count == 0 ? await _client.Stores.CreateAsync(request) : await _client.Stores.UpdateAsync(existing[0].Id!, request);
        store.ExternalId1.ShouldBe(request.ExternalId1);

        (await _client.Stores.RetrieveAsync(store.Id!, new RetrieveStoreParams { Include = [StoreInclude.Status] })).Name.ShouldBe(StoreName);
    }
}

public sealed class LiveWriteFactAttribute : FactAttribute
{
    public static readonly string? ApiKey = Environment.GetEnvironmentVariable("VOUCHERLY_API_KEY");

    public LiveWriteFactAttribute()
    {
        if (string.IsNullOrEmpty(ApiKey) || Environment.GetEnvironmentVariable("VOUCHERLY_LIVE_WRITES") != "1")
        {
            Skip = "VOUCHERLY_API_KEY and VOUCHERLY_LIVE_WRITES=1 are needed.";
        }
        else if (!ApiKey.StartsWith("sk_sand_", StringComparison.Ordinal))
        {
            Skip = "The live tests run only with a sandbox key (sk_sand_).";
        }
    }
}
