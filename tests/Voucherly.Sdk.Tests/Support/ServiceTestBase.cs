using System.Collections;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Voucherly.Sdk.Http;

namespace Voucherly.Sdk.Tests.Support;

public abstract partial class ServiceTestBase
{
    protected ServiceTestBase()
    {
        Handler = new FakeHttpMessageHandler();
        Client = new VoucherlyClient(new VoucherlyClientOptions { ApiKey = "sk_sand_test" }, new HttpClient(Handler));
    }

    protected FakeHttpMessageHandler Handler { get; }

    protected IVoucherlyClient Client { get; }

    /// <summary>
    /// Queues the success response of the operation, with a body that holds every property the spec documents.
    /// </summary>
    protected JsonNode? RespondWithSample(string operationId)
    {
        var (status, body) = SpecExamples.SuccessResponse(operationId);
        Handler.Respond((HttpStatusCode)status, body?.ToJsonString() ?? "");
        return body;
    }

    /// <summary>
    /// Asserts that the last request is the operation of the spec, with the given path parameters, query and JSON body.
    /// </summary>
    protected RecordedRequest AssertOperation(string operationId, IReadOnlyDictionary<string, string>? pathParameters = null, string query = "", JsonNode? body = null)
    {
        var route = SpecExamples.Operations()[operationId];
        var method = route[..route.IndexOf(' ')];
        var path = PathParameter().Replace(route[(route.IndexOf(' ') + 1)..], match => Uri.EscapeDataString(pathParameters![match.Groups[1].Value]));
        var request = Handler.LastRequest;

        Handler.Requests.Count.ShouldBe(1);
        request.Method.Method.ShouldBe(method);
        request.Uri.AbsoluteUri.ShouldBe("https://api.voucherly.it" + path + (query.Length > 0 ? "?" + query : ""));
        request.Headers["Voucherly-API-Key"].ShouldBe("sk_sand_test");

        if (body is null)
        {
            request.Body.ShouldBeNull();
        }
        else
        {
            request.ContentType.ShouldBe("application/json; charset=utf-8");
            AssertJsonEqual(body, JsonNode.Parse(request.Body!));
        }

        return request;
    }

    /// <summary>
    /// Asserts that the object holds every member of the JSON it was read from, and nothing else.
    /// </summary>
    protected static void AssertReadsEveryMember(JsonNode? json, VoucherlyObject value)
    {
        AssertNoExtensionData(value, "$");
        AssertJsonEqual(json, JsonNode.Parse(JsonSerializer.Serialize(value, value.GetType(), VoucherlyJson.Options)));
    }

    protected static T Deserialize<T>(JsonNode json)
        where T : class => JsonSerializer.Deserialize<T>(json.ToJsonString(), VoucherlyJson.Options).ShouldNotBeNull();

    protected static void AssertNoExtensionData(object? value, string path)
    {
        switch (value)
        {
            case VoucherlyObject voucherlyObject:
                (voucherlyObject.ExtensionData?.Keys.ToList() ?? []).ShouldBeEmpty($"Unknown members at {path}.");
                foreach (var property in value.GetType().GetProperties())
                {
                    if (property.Name != nameof(VoucherlyObject.ExtensionData) && property.GetIndexParameters().Length == 0)
                    {
                        AssertNoExtensionData(property.GetValue(value), $"{path}.{property.Name}");
                    }
                }

                break;
            case IEnumerable items and not string and not IDictionary:
                var index = 0;
                foreach (var item in items)
                {
                    AssertNoExtensionData(item, $"{path}[{index++}]");
                }

                break;
        }
    }

    /// <summary>
    /// Compares two JSON values, taking numbers by value and two date-times as equal when they are the same instant.
    /// </summary>
    protected static void AssertJsonEqual(JsonNode? expected, JsonNode? actual, string path = "$")
    {
        switch (expected)
        {
            case null:
                actual.ShouldBeNull(path);
                break;
            case JsonObject expectedObject:
                var actualObject = actual.ShouldBeOfType<JsonObject>(path);
                actualObject.Select(member => member.Key).Order().ShouldBe(expectedObject.Select(member => member.Key).Order(), path);
                foreach (var (key, value) in expectedObject)
                {
                    AssertJsonEqual(value, actualObject[key], $"{path}.{key}");
                }

                break;
            case JsonArray expectedArray:
                var actualArray = actual.ShouldBeOfType<JsonArray>(path);
                actualArray.Count.ShouldBe(expectedArray.Count, path);
                for (var i = 0; i < expectedArray.Count; i++)
                {
                    AssertJsonEqual(expectedArray[i], actualArray[i], $"{path}[{i}]");
                }

                break;
            default:
                actual.ShouldNotBeNull(path);
                var expectedElement = JsonSerializer.Deserialize<JsonElement>(expected.ToJsonString());
                var actualElement = JsonSerializer.Deserialize<JsonElement>(actual.ToJsonString());
                if (expectedElement.ValueKind == JsonValueKind.Number)
                {
                    actualElement.GetDecimal().ShouldBe(expectedElement.GetDecimal(), path);
                }
                else if (expectedElement.ValueKind == JsonValueKind.String && actualElement.ValueKind == JsonValueKind.String
                    && expectedElement.GetString()!.Contains('T') && DateTimeOffset.TryParse(expectedElement.GetString(), CultureInfo.InvariantCulture, out var expectedDate)
                    && DateTimeOffset.TryParse(actualElement.GetString(), CultureInfo.InvariantCulture, out var actualDate))
                {
                    actualDate.ShouldBe(expectedDate, path);
                }
                else
                {
                    actualElement.GetRawText().ShouldBe(expectedElement.GetRawText(), path);
                }

                break;
        }
    }

    [GeneratedRegex(@"\{(\w+)\}")]
    private static partial Regex PathParameter();
}
