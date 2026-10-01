using System.Text.Json.Serialization;
using Voucherly.Sdk.Http;
using Voucherly.Sdk.Requests;
using Voucherly.Sdk.Services;

namespace Voucherly.Sdk.Tests.Support;

public sealed class SampleObject : VoucherlyObject
{
    public string? Name { get; set { field = value; MarkAssigned(); } }

    public int? Quantity { get; set { field = value; MarkAssigned(); } }

    public double? Rate { get; set { field = value; MarkAssigned(); } }

    public Dictionary<string, string>? Metadata { get; set { field = value; MarkAssigned(); } }

    public DateOnly? Day { get; set { field = value; MarkAssigned(); } }

    public DateTimeOffset? Moment { get; set { field = value; MarkAssigned(); } }

    public SampleChild? Child { get; set { field = value; MarkAssigned(); } }

    public List<SampleChild>? Children { get; set { field = value; MarkAssigned(); } }

    [JsonPropertyName("$type")]
    public string? Kind { get; set { field = value; MarkAssigned(); } }
}

public sealed class SampleChild : VoucherlyObject
{
    public string? Code { get; set { field = value; MarkAssigned(); } }

    public int? Size { get; set { field = value; MarkAssigned(); } }
}

public sealed class SampleParams : RequestParams
{
    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public IReadOnlyList<string>? Include { get; set; }

    public DateTimeOffset? FromDate { get; set; }

    public DateOnly? Date { get; set; }

    [HeaderParam("Voucherly-Wait-Time")]
    public int? WaitTime { get; set; }

    public int? Length { get; set; }
}

internal sealed class TestService(ApiRequestor requestor) : ServiceBase(requestor)
{
    public Task<T> JsonAsync<T>(HttpMethod method, string path, VoucherlyObject? body = null, RequestParams? parameters = null, CancellationToken cancellationToken = default) =>
        Requestor.SendAsync<T>(method, path, body, parameters, cancellationToken);

    public Task<byte[]> BytesAsync(string path) => Requestor.SendBytesAsync(path, "application/pdf", CancellationToken.None);

    public Task NoContentAsync(HttpMethod method, string path) => Requestor.SendNoContentAsync(method, path, null, CancellationToken.None);

    public static string BuildPath(string format, params string[] ids) => Path(format, ids);
}

internal static class TestClients
{
    public static (TestService Service, FakeHttpMessageHandler Handler) Create(VoucherlyClientOptions? options = null)
    {
        var handler = new FakeHttpMessageHandler();
        var client = new VoucherlyClient(options ?? new VoucherlyClientOptions { ApiKey = "sk_sand_test" }, new HttpClient(handler));
        return (new TestService(client.Requestor), handler);
    }
}
