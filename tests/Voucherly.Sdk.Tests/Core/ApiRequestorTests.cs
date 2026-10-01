using System.Net;
using System.Text.Json;
using Voucherly.Sdk.Exceptions;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Core;

public class ApiRequestorTests
{
    [Fact]
    public async Task SendsAJsonBodyWithItsContentType()
    {
        var (service, handler) = TestClients.Create();
        handler.Respond(HttpStatusCode.Created, """{"id":"pay_1"}""");

        var result = await service.JsonAsync<JsonElement>(HttpMethod.Post, "/v1/things", new SampleObject { Name = "Caffè €" });

        result.GetProperty("id").GetString().ShouldBe("pay_1");
        var request = handler.LastRequest;
        request.Method.ShouldBe(HttpMethod.Post);
        request.Uri.ShouldBe(new Uri("https://api.voucherly.it/v1/things"));
        request.Body.ShouldBe("""{"name":"Caffè €"}""");
        request.ContentType.ShouldBe("application/json; charset=utf-8");
        request.Headers["Accept"].ShouldBe("application/json");
    }

    [Fact]
    public async Task SendsAnEmptyJsonObjectOnAPostWithoutARequestObject()
    {
        var (service, handler) = TestClients.Create();
        handler.Respond(HttpStatusCode.OK, "{}");

        await service.JsonAsync<JsonElement>(HttpMethod.Post, "/v1/things/1/void");

        handler.LastRequest.Body.ShouldBe("{}");
        handler.LastRequest.ContentType.ShouldBe("application/json; charset=utf-8");
    }

    [Fact]
    public async Task SendsNoBodyOnAGet()
    {
        var (service, handler) = TestClients.Create();
        handler.Respond(HttpStatusCode.OK, "{}");

        await service.JsonAsync<JsonElement>(HttpMethod.Get, "/v1/things/1");

        handler.LastRequest.Body.ShouldBeNull();
    }

    [Fact]
    public async Task AppendsTheQueryAndTheHeaderParameters()
    {
        var (service, handler) = TestClients.Create();
        handler.Respond(HttpStatusCode.OK, "{}");

        await service.JsonAsync<JsonElement>(HttpMethod.Get, "/v1/things/1", parameters: new SampleParams { Include = ["Lines"], WaitTime = 10 });

        handler.LastRequest.Uri.ShouldBe(new Uri("https://api.voucherly.it/v1/things/1?include=Lines"));
        handler.LastRequest.Headers["Voucherly-Wait-Time"].ShouldBe("10");
    }

    [Fact]
    public async Task ReturnsTheBytesOfADownload()
    {
        var (service, handler) = TestClients.Create();
        byte[] pdf = [0x25, 0x50, 0x44, 0x46, 0x00, 0xFF];
        handler.Respond(HttpStatusCode.OK, pdf, "application/pdf");

        var bytes = await service.BytesAsync("/v1/receipts/rcp_1/download");

        bytes.ShouldBe(pdf);
        handler.LastRequest.Headers["Accept"].ShouldBe("application/pdf");
    }

    [Fact]
    public async Task AcceptsAnEmptyNoContentResponse()
    {
        var (service, handler) = TestClients.Create();
        handler.Respond(HttpStatusCode.NoContent);

        await service.NoContentAsync(HttpMethod.Delete, "/v1/things/1");

        handler.LastRequest.Method.ShouldBe(HttpMethod.Delete);
    }

    [Fact]
    public async Task RejectsAResponseThatIsNotJson()
    {
        var (service, handler) = TestClients.Create();
        handler.Respond(HttpStatusCode.OK, "<html>");

        var exception = await Should.ThrowAsync<VoucherlyException>(() => service.JsonAsync<SampleObject>(HttpMethod.Get, "/v1/things"));

        exception.Message.ShouldStartWith("The response body is not valid JSON", Case.Sensitive);
    }

    [Fact]
    public async Task WrapsANetworkFailureInAConnectionException()
    {
        var (service, handler) = TestClients.Create();
        handler.Fail(new HttpRequestException("No such host is known."));

        var exception = await Should.ThrowAsync<ConnectionException>(() => service.JsonAsync<SampleObject>(HttpMethod.Get, "/v1/things"));

        exception.InnerException.ShouldBeOfType<HttpRequestException>();
    }

    [Fact]
    public async Task WrapsATimeoutInAConnectionException()
    {
        var handler = new FakeHttpMessageHandler().Hang();
        var client = new VoucherlyClient(new VoucherlyClientOptions { ApiKey = "sk_sand_test" }, new HttpClient(handler) { Timeout = TimeSpan.FromMilliseconds(100) });
        var service = new TestService(client.Requestor);

        await Should.ThrowAsync<ConnectionException>(() => service.JsonAsync<SampleObject>(HttpMethod.Get, "/v1/things"));
    }

    [Fact]
    public async Task LetsTheCancellationOfTheCallerThrough()
    {
        var (service, handler) = TestClients.Create();
        handler.Hang();
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

        var exception = await Should.ThrowAsync<OperationCanceledException>(() => service.JsonAsync<SampleObject>(HttpMethod.Get, "/v1/things", cancellationToken: cancellation.Token));

        exception.ShouldNotBeAssignableTo<VoucherlyException>();
    }

    [Fact]
    public void EncodesEachIdAsAPathSegment()
    {
        TestService.BuildPath("/v1/customers/{0}/addresses/{1}", "cs/1", "a b").ShouldBe("/v1/customers/cs%2F1/addresses/a%20b");
    }

    [Fact]
    public void RejectsAnEmptyId()
    {
        Should.Throw<ArgumentException>(() => TestService.BuildPath("/v1/payments/{0}", ""));
    }
}
