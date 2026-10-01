using Voucherly.Sdk.Enums;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Services;

public class StoreServicesTests : ServiceTestBase
{
    [Fact]
    public async Task ListStores()
    {
        var json = RespondWithSample("list-store");

        var page = await Client.Stores.ListAsync(new ListStoreParams
        {
            Name = "Milano",
            IsActive = true,
            ConceptStoreId = "conce_1",
            StoreAreaId = "starea_1",
            Include = [StoreInclude.ConceptStore, StoreInclude.Status],
        });

        AssertOperation("list-store", query: "name=Milano&isActive=true&conceptStoreId=conce_1&storeAreaId=starea_1&include=conceptStore&include=status");
        page.Items[0].ShouldBeOfType<Store>();
        AssertReadsEveryMember(json, page);
    }

    [Fact]
    public async Task CreateStore()
    {
        var json = RespondWithSample("create-store");
        var body = SpecExamples.RequestExamples("create-store")["default"];

        var store = await Client.Stores.CreateAsync(Deserialize<StoreRequest>(body));

        AssertOperation("create-store", body: body);
        AssertReadsEveryMember(json, store);
    }

    [Fact]
    public async Task RetrieveStore()
    {
        var json = RespondWithSample("retrieve-store");

        var store = await Client.Stores.RetrieveAsync("sto_1", new RetrieveStoreParams { Include = [StoreInclude.StoreArea] });

        AssertOperation("retrieve-store", Id("sto_1"), "include=storeArea");
        AssertReadsEveryMember(json, store);
    }

    [Fact]
    public async Task UpdateStore()
    {
        var json = RespondWithSample("update-store");
        var body = SpecExamples.RequestExamples("update-store")["default"];

        var store = await Client.Stores.UpdateAsync("sto_1", Deserialize<StoreRequest>(body));

        AssertOperation("update-store", Id("sto_1"), body: body);
        AssertReadsEveryMember(json, store);
    }

    [Fact]
    public async Task ListConceptStores()
    {
        var json = RespondWithSample("list-concept-store");

        var page = await Client.ConceptStores.ListAsync(new ListConceptStoreParams { Name = "Bistrot", Ids = ["conce_1", "conce_2"] });

        AssertOperation("list-concept-store", query: "name=Bistrot&ids=conce_1&ids=conce_2");
        page.Items[0].ShouldBeOfType<ConceptStore>();
        AssertReadsEveryMember(json, page);
    }

    [Fact]
    public async Task CreateConceptStore()
    {
        var json = RespondWithSample("create-concept-store");
        var body = SpecExamples.RequestExamples("create-concept-store")["default"];

        var conceptStore = await Client.ConceptStores.CreateAsync(Deserialize<ConceptStoreRequest>(body));

        AssertOperation("create-concept-store", body: body);
        AssertReadsEveryMember(json, conceptStore);
    }

    [Fact]
    public async Task RetrieveConceptStore()
    {
        var json = RespondWithSample("retrieve-concept-store");

        var conceptStore = await Client.ConceptStores.RetrieveAsync("conce_1");

        AssertOperation("retrieve-concept-store", Id("conce_1"));
        AssertReadsEveryMember(json, conceptStore);
    }

    [Fact]
    public async Task UpdateConceptStore()
    {
        var json = RespondWithSample("update-concept-store");
        var body = SpecExamples.RequestExamples("update-concept-store")["default"];

        var conceptStore = await Client.ConceptStores.UpdateAsync("conce_1", Deserialize<ConceptStoreRequest>(body));

        AssertOperation("update-concept-store", Id("conce_1"), body: body);
        AssertReadsEveryMember(json, conceptStore);
    }

    [Fact]
    public async Task DeleteConceptStoreMigratingItsStores()
    {
        RespondWithSample("delete-concept-store");

        await Client.ConceptStores.DeleteAsync("conce_1", new DeleteConceptStoreParams { MigrateToConceptStoreId = "conce_2" });

        AssertOperation("delete-concept-store", Id("conce_1"), "migrateToConceptStoreId=conce_2");
    }

    [Fact]
    public async Task DeleteConceptStore()
    {
        RespondWithSample("delete-concept-store");

        await Client.ConceptStores.DeleteAsync("conce_1");

        AssertOperation("delete-concept-store", Id("conce_1"));
    }

    [Fact]
    public async Task ListStoreAreas()
    {
        var json = RespondWithSample("list-store-area");

        var page = await Client.StoreAreas.ListAsync(new ListStoreAreaParams { Ids = ["starea_1"], Start = "starea_next" });

        AssertOperation("list-store-area", query: "ids=starea_1&start=starea_next");
        page.Items[0].ShouldBeOfType<StoreArea>();
        AssertReadsEveryMember(json, page);
    }

    [Fact]
    public async Task CreateStoreArea()
    {
        var json = RespondWithSample("create-store-area");
        var body = SpecExamples.RequestExamples("create-store-area")["default"];

        var storeArea = await Client.StoreAreas.CreateAsync(Deserialize<StoreAreaRequest>(body));

        AssertOperation("create-store-area", body: body);
        AssertReadsEveryMember(json, storeArea);
    }

    [Fact]
    public async Task RetrieveStoreArea()
    {
        var json = RespondWithSample("retrieve-store-area");

        var storeArea = await Client.StoreAreas.RetrieveAsync("starea_1");

        AssertOperation("retrieve-store-area", Id("starea_1"));
        AssertReadsEveryMember(json, storeArea);
    }

    [Fact]
    public async Task UpdateStoreArea()
    {
        var json = RespondWithSample("update-store-area");
        var body = SpecExamples.RequestExamples("update-store-area")["default"];

        var storeArea = await Client.StoreAreas.UpdateAsync("starea_1", Deserialize<StoreAreaRequest>(body));

        AssertOperation("update-store-area", Id("starea_1"), body: body);
        AssertReadsEveryMember(json, storeArea);
    }

    [Fact]
    public async Task DeleteStoreAreaMigratingItsStores()
    {
        RespondWithSample("delete-store-area");

        await Client.StoreAreas.DeleteAsync("starea_1", new DeleteStoreAreaParams { MigrateToStoreAreaId = "starea_2" });

        AssertOperation("delete-store-area", Id("starea_1"), "migrateToStoreAreaId=starea_2");
    }

    private static Dictionary<string, string> Id(string id) => new() { ["id"] = id };
}
