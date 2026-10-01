using System.Globalization;
using System.Reflection;
using System.Text.Json.Nodes;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests;

/// <summary>
/// Checks the classes against the schemas of the spec, so that a changed spec fails here until the SDK follows it.
/// </summary>
public class SchemaCoverageTests : ServiceTestBase
{
    private static readonly string[] Groups = ["Payments", "Customers", "Stores", "PaymentGateways", "Receipts", "Terminals", "Reports"];

    private static readonly Dictionary<string, string> Renames = new()
    {
        ["CompanyAddressForExternalApi"] = "CompanyAddress",
        ["PaymentGateways.GetPaymentGatewaysResponse"] = "PaymentGatewayList",
        ["PaginationResponse"] = "Pagination",
    };

    private static readonly Dictionary<string, string> InlineEnums = new()
    {
        ["Company.packagingType"] = "PackagingType",
        ["Customers.Packaging.type"] = "PackagingType",
        ["CreatePaymentRequest.completionMode"] = "CompletionMode",
        ["CreatePaymentRequest.exceedingAmountMode"] = "ExceedingAmountMode",
        ["Stores.StoreStatus.status"] = "StoreStatusValue",
    };

    private static readonly Dictionary<string, string> IncludeEnums = new()
    {
        ["retrieve-company"] = "CompanyInclude",
        ["retrieve-customer"] = "CustomerInclude",
        ["retrieve-payment"] = "PaymentInclude",
        ["list-payment-gateway"] = "PaymentGatewayInclude",
    };

    public static TheoryData<string, string> RequestBodies()
    {
        var data = new TheoryData<string, string>();
        foreach (var operationId in SpecExamples.Operations().Keys)
        {
            if (SpecExamples.Operation(operationId)["requestBody"] is null)
            {
                continue;
            }

            data.Add(operationId, "schema");
            foreach (var name in SpecExamples.RequestExamples(operationId).Keys)
            {
                data.Add(operationId, name);
            }
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(RequestBodies))]
    public void EveryRequestBodyAndExampleRoundTripsThroughItsClass(string operationId, string example)
    {
        var schema = SpecExamples.Operation(operationId)["requestBody"]!["content"]!["application/json"]!["schema"]!;
        var reference = schema["$ref"]!.GetValue<string>();
        var type = typeof(VoucherlyClient).Assembly.GetType("Voucherly.Sdk.Requests." + ClassName(reference[(reference.LastIndexOf('/') + 1)..]), throwOnError: true)!;
        var json = example == "schema" ? SpecExamples.Sample(schema)! : SpecExamples.RequestExamples(operationId)[example];

        var request = (VoucherlyObject)System.Text.Json.JsonSerializer.Deserialize(json.ToJsonString(), type, Http.VoucherlyJson.Options)!;

        AssertReadsEveryMember(json, request);
    }

    [Fact]
    public void EveryEnumHasTheValuesOfTheSpec()
    {
        var enums = new Dictionary<string, List<string>>();
        foreach (var (key, schema) in SpecExamples.Spec["components"]!["schemas"]!.AsObject())
        {
            if (schema!["enum"] is JsonArray values)
            {
                enums[ClassName(key)] = Values(values);
            }

            foreach (var (path, inline) in FindInlineEnums(key, schema))
            {
                InlineEnums.ContainsKey(path).ShouldBeTrue($"The inline enum {path} needs a class name.");
                enums[InlineEnums[path]] = Values(inline);
            }
        }

        foreach (var operationId in SpecExamples.Operations().Keys)
        {
            foreach (var parameter in SpecExamples.Operation(operationId)["parameters"]?.AsArray() ?? [])
            {
                if (parameter?["schema"]?["items"]?["enum"] is JsonArray values)
                {
                    IncludeEnums.ContainsKey(operationId).ShouldBeTrue($"The enum of {operationId} needs a class name.");
                    enums[IncludeEnums[operationId]] = Values(values);
                }
            }
        }

        foreach (var (name, values) in enums)
        {
            var type = typeof(VoucherlyClient).Assembly.GetType("Voucherly.Sdk.Enums." + name, throwOnError: true)!;
            var constants = type.GetFields(BindingFlags.Public | BindingFlags.Static).Select(field => Convert.ToString(field.GetRawConstantValue(), CultureInfo.InvariantCulture)!);
            constants.Order().ShouldBe(values.Order(), name);
        }
    }

    [Fact]
    public void EveryParameterOfTheSpecHasAProperty()
    {
        foreach (var operationId in SpecExamples.Operations().Keys)
        {
            var expected = new List<string>();
            foreach (var node in SpecExamples.Operation(operationId)["parameters"]?.AsArray() ?? [])
            {
                var parameter = SpecExamples.Resolve(node!);
                var name = parameter["name"]!.GetValue<string>();
                switch (parameter["in"]!.GetValue<string>())
                {
                    case "query":
                        expected.Add(char.ToUpperInvariant(name[0]) + name[1..]);
                        break;
                    case "header":
                        expected.Add(string.Concat(name.Replace("Voucherly-", "").Split('-').Select(word => char.ToUpperInvariant(word[0]) + word[1..].ToLowerInvariant())));
                        break;
                }
            }

            var className = "Voucherly.Sdk.Requests." + string.Concat(operationId.Split('-').Select(word => char.ToUpperInvariant(word[0]) + word[1..])) + "Params";
            var type = typeof(VoucherlyClient).Assembly.GetType(className);
            if (expected.Count == 0)
            {
                type.ShouldBeNull($"{className} has no parameter to hold.");
                continue;
            }

            type.ShouldNotBeNull(className).GetProperties().Select(property => property.Name).Order().ShouldBe(expected.Order(), className);
        }
    }

    [Fact]
    public void EveryRequestPropertyHasTheNullabilityOfTheSpec()
    {
        var context = new NullabilityInfoContext();
        var refused = 0;
        foreach (var (key, schema) in SpecExamples.Spec["components"]!["schemas"]!.AsObject())
        {
            refused += AssertNullability(context, ClassName(key), schema!);
        }

        refused.ShouldBeGreaterThan(0);
    }

    /// <summary>
    /// The class of a schema: the name without its group prefix, with the remaining segments joined.
    /// </summary>
    private static string ClassName(string schemaKey)
    {
        if (Renames.TryGetValue(schemaKey, out var renamed))
        {
            return renamed;
        }

        var segments = schemaKey.Split('.').AsEnumerable();
        if (Groups.Contains(segments.First()))
        {
            segments = segments.Skip(1);
        }

        return string.Concat(segments.Select(segment => char.ToUpperInvariant(segment[0]) + segment[1..]));
    }

    /// <summary>
    /// Checks the properties of a class used by requests alone, and of its inline objects, and returns how many refuse null because the spec says <c>nullable: false</c>.
    /// A response may leave any property out, so the classes of <c>Voucherly.Sdk.Models</c> are not checked.
    /// </summary>
    private static int AssertNullability(NullabilityInfoContext context, string className, JsonNode schema)
    {
        var type = typeof(VoucherlyClient).Assembly.GetType("Voucherly.Sdk.Requests." + className);
        if (type is null)
        {
            return 0;
        }

        var refused = 0;
        foreach (var (name, property) in schema["properties"]?.AsObject() ?? [])
        {
            var propertyName = name == "$type" ? "Type" : char.ToUpperInvariant(name[0]) + name[1..];
            var clrProperty = type.GetProperty(propertyName).ShouldNotBeNull($"{className}.{propertyName}");
            var nullable = property!["nullable"]?.GetValue<bool>();
            var expected = nullable == false || (nullable is null && clrProperty.PropertyType.IsValueType) ? NullabilityState.NotNull : NullabilityState.Nullable;
            context.Create(clrProperty).WriteState.ShouldBe(expected, $"{className}.{propertyName}");
            refused += nullable == false ? 1 : 0;

            var inner = property["type"]?.GetValue<string>() == "array" ? property["items"]! : property;
            if (inner["type"]?.GetValue<string>() == "object")
            {
                refused += AssertNullability(context, className + propertyName, inner);
            }
        }

        return refused;
    }

    private static IEnumerable<(string Path, JsonArray Values)> FindInlineEnums(string path, JsonNode? schema)
    {
        foreach (var (name, property) in schema?["properties"]?.AsObject() ?? [])
        {
            if (property?["enum"] is JsonArray values)
            {
                yield return ($"{path}.{name}", values);
            }

            foreach (var nested in FindInlineEnums($"{path}.{name}", property))
            {
                yield return nested;
            }
        }
    }

    private static List<string> Values(JsonArray values) => values.Select(value => value!.ToJsonString().Trim('"')).ToList();
}
