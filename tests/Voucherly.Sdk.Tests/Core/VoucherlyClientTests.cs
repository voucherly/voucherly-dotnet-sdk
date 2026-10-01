using System.Net;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Extensions.DependencyInjection;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Core;

public class VoucherlyClientTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void RequiresTheApiKey(string apiKey)
    {
        Should.Throw<ArgumentException>(() => new VoucherlyClient(new VoucherlyClientOptions { ApiKey = apiKey }));
    }

    [Fact]
    public async Task SendsTheKeyAndTheUserAgentAndNoPlatformHeaderByDefault()
    {
        var request = await SendThrough(new VoucherlyClientOptions { ApiKey = "sk_sand_test", Os = null, OsVersion = null, OsFramework = null, App = null, AppVersion = null, AppHouse = null });

        request.Uri.ShouldBe(new Uri("https://api.voucherly.it/v1/things"));
        request.Headers.Keys.Order().ShouldBe(["Accept", "User-Agent", "Voucherly-API-Key"]);
        request.Headers["Voucherly-API-Key"].ShouldBe("sk_sand_test");
        request.Headers["User-Agent"].ShouldBe($"VoucherlyApiDotnetSdk/{VoucherlyClient.Version}");
    }

    [Fact]
    public async Task SendsThePlatformAndTelemetryHeadersThatHaveAValue()
    {
        var request = await SendThrough(new VoucherlyClientOptions
        {
            ApiKey = "ik_test",
            MerchantId = "b3c4a1d2-0000-4000-8000-000000000001",
            Tenant = "sand",
            BaseUrl = new Uri("http://localhost:5000/"),
            Os = "Windows",
            OsVersion = "10.0.26200",
            OsFramework = "",
            App = "Erbert",
            AppVersion = "3.1.0",
            AppHouse = "Voucherly",
            DeviceType = null,
        });

        request.Uri.ShouldBe(new Uri("http://localhost:5000/v1/things"));
        request.Headers["Voucherly-Merchant-Id"].ShouldBe("b3c4a1d2-0000-4000-8000-000000000001");
        request.Headers["Voucherly-Tenant"].ShouldBe("sand");
        request.Headers["x-voucherly-os"].ShouldBe("Windows");
        request.Headers["x-voucherly-osversion"].ShouldBe("10.0.26200");
        request.Headers["x-voucherly-app"].ShouldBe("Erbert");
        request.Headers["x-voucherly-appversion"].ShouldBe("3.1.0");
        request.Headers["x-voucherly-apphouse"].ShouldBe("Voucherly");
        request.Headers.ContainsKey("x-voucherly-osframework").ShouldBeFalse();
        request.Headers.ContainsKey("x-voucherly-devicetype").ShouldBeFalse();
    }

    [Fact]
    public async Task LeavesTheInjectedHttpClientUntouched()
    {
        var handler = new FakeHttpMessageHandler().Respond(HttpStatusCode.OK, "{}");
        using var httpClient = new HttpClient(handler);
        var client = new VoucherlyClient(new VoucherlyClientOptions { ApiKey = "sk_sand_test" }, httpClient);

        await new TestService(client.Requestor).JsonAsync<JsonElement>(HttpMethod.Get, "/v1/things");

        httpClient.BaseAddress.ShouldBeNull();
        httpClient.DefaultRequestHeaders.ShouldBeEmpty();
    }

    [Fact]
    public async Task AddVoucherlyRegistersTheClientAndLetsTheCallerAddHandlers()
    {
        var handler = new FakeHttpMessageHandler().Respond(HttpStatusCode.OK, "{}");
        var services = new ServiceCollection();
        services.AddVoucherly(options => options.ApiKey = "sk_sand_di")
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        using var provider = services.BuildServiceProvider();

        var client = provider.GetRequiredService<IVoucherlyClient>().ShouldBeOfType<VoucherlyClient>();
        await new TestService(client.Requestor).JsonAsync<JsonElement>(HttpMethod.Get, "/v1/things");

        handler.LastRequest.Headers["Voucherly-API-Key"].ShouldBe("sk_sand_di");
    }

    [Fact]
    public void TheVersionIsTheOneOfThePackage()
    {
        var project = XDocument.Load(Path.Combine(RepositoryRoot(), "src", "Voucherly.Sdk", "Voucherly.Sdk.csproj"));

        VoucherlyClient.Version.ShouldBe(project.Descendants("Version").Single().Value);
    }

    [Fact]
    public void TheChangelogHasAnEntryForTheVersion()
    {
        File.ReadAllText(Path.Combine(RepositoryRoot(), "CHANGELOG.md")).ShouldContain($"## {VoucherlyClient.Version} - ", Case.Sensitive);
    }

    private static async Task<RecordedRequest> SendThrough(VoucherlyClientOptions options)
    {
        var (service, handler) = TestClients.Create(options);
        handler.Respond(HttpStatusCode.OK, "{}");

        await service.JsonAsync<JsonElement>(HttpMethod.Get, "/v1/things");

        return handler.LastRequest;
    }

    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Voucherly.Sdk.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new InvalidOperationException("The repository root was not found.");
    }
}
