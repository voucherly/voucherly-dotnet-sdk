using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Voucherly.Sdk.Tests.Support;

public static partial class SpecExamples
{
    private static readonly string[] HttpMethods = ["get", "post", "put", "patch", "delete"];

    private static readonly Lazy<JsonObject> LazySpec = new(() =>
    {
        // YamlDotNet rejects a literal block whose first line holds only spaces, which the spec has and other parsers accept.
        var yaml = TrailingWhitespace().Replace(File.ReadAllText(SpecFile("openapi.yaml")), "");
        var stream = new YamlStream();
        stream.Load(new StringReader(yaml));

        return (JsonObject)ToJson(stream.Documents[0].RootNode)!;
    });

    public static JsonObject Spec => LazySpec.Value;

    public static string SpecFile(string name) => Path.Combine(AppContext.BaseDirectory, "spec", name);

    /// <summary>
    /// Every operation of the spec, by operationId, as "METHOD /path".
    /// </summary>
    public static Dictionary<string, string> Operations()
    {
        var operations = new Dictionary<string, string>();
        foreach (var (path, item) in Spec["paths"]!.AsObject())
        {
            foreach (var (method, operation) in item!.AsObject())
            {
                if (HttpMethods.Contains(method) && operation?["operationId"] is { } operationId)
                {
                    operations[operationId.GetValue<string>()] = $"{method.ToUpperInvariant()} {path}";
                }
            }
        }

        return operations;
    }

    public static JsonObject Operation(string operationId)
    {
        foreach (var (_, item) in Spec["paths"]!.AsObject())
        {
            foreach (var (method, operation) in item!.AsObject())
            {
                if (HttpMethods.Contains(method) && operation?["operationId"]?.GetValue<string>() == operationId)
                {
                    return operation.AsObject();
                }
            }
        }

        throw new ArgumentException($"The spec has no operation {operationId}.");
    }

    /// <summary>
    /// The request body examples of an operation, by name; an inline example is named "default".
    /// </summary>
    public static Dictionary<string, JsonNode> RequestExamples(string operationId) =>
        Examples(Operation(operationId)["requestBody"]?["content"]?["application/json"]?.AsObject());

    /// <summary>
    /// Every example of every error response declared by the spec.
    /// </summary>
    public static IEnumerable<(string OperationId, int Status, string Name, JsonNode Body)> ErrorExamples()
    {
        foreach (var operationId in Operations().Keys)
        {
            foreach (var (status, response) in Operation(operationId)["responses"]!.AsObject())
            {
                if (int.Parse(status, CultureInfo.InvariantCulture) < 400)
                {
                    continue;
                }

                foreach (var (name, body) in Examples(Resolve(response!)["content"]?["application/json"]?.AsObject()))
                {
                    yield return (operationId, int.Parse(status, CultureInfo.InvariantCulture), name, body);
                }
            }
        }
    }

    /// <summary>
    /// The success status of an operation, with a body built from its schema that holds every property the spec documents.
    /// </summary>
    public static (int Status, JsonNode? Body) SuccessResponse(string operationId)
    {
        foreach (var (status, response) in Operation(operationId)["responses"]!.AsObject())
        {
            var code = int.Parse(status, CultureInfo.InvariantCulture);
            if (code is >= 200 and < 300)
            {
                var schema = response?["content"]?["application/json"]?["schema"];
                return (code, schema is null ? null : Sample(schema));
            }
        }

        throw new ArgumentException($"The operation {operationId} has no success response.");
    }

    /// <summary>
    /// A value for the schema: its example when it has one, otherwise a value of its type, with every property of an object.
    /// </summary>
    public static JsonNode? Sample(JsonNode schemaNode)
    {
        var schema = Resolve(schemaNode).AsObject();
        if (schema["allOf"] is JsonArray allOf)
        {
            return Sample(allOf[0]!);
        }
        if (schema["enum"] is JsonArray values)
        {
            return values[0]?.DeepClone();
        }
        if (schema.TryGetPropertyValue("example", out var example))
        {
            return example?.DeepClone();
        }

        switch (schema["type"]?.GetValue<string>() ?? "object")
        {
            case "array":
                return new JsonArray(Sample(schema["items"]!));
            case "integer":
                return JsonValue.Create(1);
            case "number":
                return JsonValue.Create(22.5);
            case "boolean":
                return JsonValue.Create(true);
            case "string":
                return JsonValue.Create(schema["format"]?.GetValue<string>() switch
                {
                    "date-time" => "2026-09-30T10:15:00Z",
                    "date" => "2026-09-30",
                    "uuid" => "0b4c6f3e-2d1a-4c5b-9e8f-7a6b5c4d3e2f",
                    "uri" => "https://example.com/pos",
                    _ => "text",
                });
            default:
                if (schema["properties"] is not JsonObject properties)
                {
                    return new JsonObject { ["key"] = schema["additionalProperties"] is JsonObject additional ? Sample(additional) : JsonValue.Create("value") };
                }

                var sample = new JsonObject();
                foreach (var (name, property) in properties)
                {
                    sample[name] = Sample(property!);
                }

                return sample;
        }
    }

    private static Dictionary<string, JsonNode> Examples(JsonObject? content)
    {
        var examples = new Dictionary<string, JsonNode>();
        if (content is null)
        {
            return examples;
        }

        if (content["example"] is { } example)
        {
            examples["default"] = example.DeepClone();
        }

        foreach (var (name, named) in content["examples"]?.AsObject() ?? [])
        {
            examples[name] = Resolve(named!)["value"]!.DeepClone();
        }

        return examples;
    }

    public static JsonNode Resolve(JsonNode node)
    {
        while (node is JsonObject reference && reference["$ref"]?.GetValue<string>() is { } target)
        {
            node = target[2..].Split('/').Aggregate((JsonNode)Spec, (current, segment) => current[segment]!);
        }

        return node;
    }

    private static JsonNode? ToJson(YamlNode node) => node switch
    {
        YamlMappingNode mapping => new JsonObject(mapping.Children.Select(child => KeyValuePair.Create(((YamlScalarNode)child.Key).Value!, ToJson(child.Value)))),
        YamlSequenceNode sequence => new JsonArray(sequence.Children.Select(ToJson).ToArray()),
        YamlScalarNode scalar => ScalarToJson(scalar),
        _ => throw new NotSupportedException(node.GetType().Name),
    };

    private static JsonNode? ScalarToJson(YamlScalarNode scalar)
    {
        var value = scalar.Value ?? "";
        if (scalar.Style != ScalarStyle.Plain)
        {
            return JsonValue.Create(value);
        }

        if (value is "" or "~" or "null" or "Null" or "NULL")
        {
            return null;
        }

        if (value is "true" or "True" or "TRUE" or "false" or "False" or "FALSE")
        {
            return JsonValue.Create(bool.Parse(value));
        }

        if (IntegerPattern().IsMatch(value))
        {
            return JsonValue.Create(long.Parse(value, CultureInfo.InvariantCulture));
        }

        if (FloatPattern().IsMatch(value))
        {
            return JsonValue.Create(double.Parse(value, CultureInfo.InvariantCulture));
        }

        return JsonValue.Create(value);
    }

    [GeneratedRegex(@"[ \t]+(?=\r?$)", RegexOptions.Multiline)]
    private static partial Regex TrailingWhitespace();

    [GeneratedRegex(@"^[-+]?[0-9]+$")]
    private static partial Regex IntegerPattern();

    [GeneratedRegex(@"^[-+]?(\.[0-9]+|[0-9]+(\.[0-9]*)?)([eE][-+]?[0-9]+)?$")]
    private static partial Regex FloatPattern();
}
