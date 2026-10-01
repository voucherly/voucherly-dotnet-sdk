#:package YamlDotNet@16.3.0

// Writes src/Voucherly.Sdk/Models, Requests and Enums from spec/openapi.yaml: the models, the request bodies, the query and header parameters and the enums.
// The services, the pages and the rest of the SDK are written by hand.
// Run it with `dotnet run tools/generate.cs` after every change of the spec, then run the tests.

using System.Globalization;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

var root = FindRoot();
var sdk = Path.Combine(root, "src", "Voucherly.Sdk");
var generator = new Generator(LoadSpec(Path.Combine(root, "spec", "openapi.yaml")));
var written = generator.Run(sdk);

var handwritten = new[] { "Models/Page.cs", "Models/Pagination.cs", "Requests/RequestParams.cs" };
foreach (var directory in new[] { "Models", "Requests", "Enums" })
{
    foreach (var file in Directory.GetFiles(Path.Combine(sdk, directory), "*.cs"))
    {
        var relative = $"{directory}/{Path.GetFileName(file)}";
        if (!written.Contains(relative) && !handwritten.Contains(relative))
        {
            File.Delete(file);
            Console.WriteLine($"Removed {relative}, which the spec no longer declares.");
        }
    }
}

Console.WriteLine($"{generator.ClassCount} classes, {generator.EnumCount} enums, {written.Count - generator.ClassCount - generator.EnumCount} parameter classes written.");

static string FindRoot()
{
    var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Voucherly.Sdk.sln")))
    {
        directory = directory.Parent;
    }

    return directory?.FullName ?? throw new InvalidOperationException("Run the generator from inside the repository.");
}

static JsonObject LoadSpec(string path)
{
    // YamlDotNet rejects a literal block whose first line holds only spaces, which the spec has and other parsers accept.
    var yaml = Regex.Replace(File.ReadAllText(path), @"[ \t]+(?=\r?$)", "", RegexOptions.Multiline);
    var stream = new YamlStream();
    stream.Load(new StringReader(yaml));
    return (JsonObject)ToJson(stream.Documents[0].RootNode)!;
}

static JsonNode? ToJson(YamlNode node) => node switch
{
    YamlMappingNode mapping => new JsonObject(mapping.Children.Select(child => KeyValuePair.Create(((YamlScalarNode)child.Key).Value!, ToJson(child.Value)))),
    YamlSequenceNode sequence => new JsonArray(sequence.Children.Select(ToJson).ToArray()),
    YamlScalarNode scalar => ScalarToJson(scalar),
    _ => throw new NotSupportedException(node.GetType().Name),
};

static JsonNode? ScalarToJson(YamlScalarNode scalar)
{
    var value = scalar.Value ?? "";
    if (scalar.Style != ScalarStyle.Plain)
    {
        return JsonValue.Create(value);
    }

    return value switch
    {
        "" or "~" or "null" or "Null" or "NULL" => null,
        "true" or "True" or "TRUE" => JsonValue.Create(true),
        "false" or "False" or "FALSE" => JsonValue.Create(false),
        _ when Regex.IsMatch(value, @"^[-+]?[0-9]+$") => JsonValue.Create(long.Parse(value, CultureInfo.InvariantCulture)),
        _ when Regex.IsMatch(value, @"^[-+]?(\.[0-9]+|[0-9]+(\.[0-9]*)?)([eE][-+]?[0-9]+)?$") => JsonValue.Create(double.Parse(value, CultureInfo.InvariantCulture)),
        _ => JsonValue.Create(value),
    };
}

sealed class Generator(JsonObject spec)
{
    private static readonly string[] Groups = ["Payments", "Customers", "Stores", "PaymentGateways", "Receipts", "Terminals", "Reports"];

    private static readonly Dictionary<string, string> Renames = new()
    {
        ["CompanyAddressForExternalApi"] = "CompanyAddress",
        ["PaymentGateways.GetPaymentGatewaysResponse"] = "PaymentGatewayList",
    };

    // Schemas that are not classes of their own: Id is a string, Metadata a map, the problem details are read by the exceptions.
    private static readonly string[] Skip = ["Id", "Metadata", "ProblemDetails", "ValidationProblemDetails", "PaymentConflictProblemDetails", "PaginationResponse"];

    private static readonly Dictionary<string, string> Handwritten = new()
    {
        ["PaginationResponse"] = "Pagination",
    };

    // An enum declared inside a property has no name in the spec: a new one must be named here, and in SchemaCoverageTests.
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

    private static readonly string[] Methods = ["get", "post", "put", "patch", "delete"];

    private readonly JsonObject _schemas = spec["components"]!["schemas"]!.AsObject();
    private readonly Dictionary<string, ClassDefinition> _classes = [];
    private readonly Dictionary<string, EnumDefinition> _enums = [];
    private readonly Dictionary<string, string> _namespaces = [];

    public int ClassCount => _classes.Count;

    public int EnumCount => _enums.Count;

    public HashSet<string> Run(string sdk)
    {
        foreach (var (key, _) in _schemas)
        {
            VisitRef(key);
        }

        AddClass("VolumesReport", "VolumesReport", spec["paths"]!["/v1/reports/volumes"]!["get"]!["responses"]!["200"]!["content"]!["application/json"]!["schema"]!.AsObject());

        foreach (var (operationId, operation) in Operations())
        {
            foreach (var parameter in operation["parameters"]?.AsArray() ?? [])
            {
                if (parameter?["name"]?.GetValue<string>() != "include")
                {
                    continue;
                }

                var items = parameter["schema"]!["items"]!.AsObject();
                if (items["enum"] is not null)
                {
                    AddEnum(IncludeEnums.TryGetValue(operationId, out var name) ? name : throw new InvalidOperationException($"The include enum of {operationId} needs a name in IncludeEnums."), items, "");
                }
                else
                {
                    VisitRef(RefName(items["$ref"]!.GetValue<string>()));
                }
            }
        }

        AssignNamespaces();

        var written = new HashSet<string>();
        foreach (var (name, definition) in _classes)
        {
            var ns = _namespaces[name] == "Model" ? "Models" : "Requests";
            Write(Path.Combine(sdk, ns, name + ".cs"), ClassFile(name, definition));
            written.Add($"{ns}/{name}.cs");
        }

        foreach (var (name, definition) in _enums)
        {
            Write(Path.Combine(sdk, "Enums", name + ".cs"), EnumFile(name, definition));
            written.Add($"Enums/{name}.cs");
        }

        foreach (var (operationId, operation) in Operations())
        {
            var parameters = (operation["parameters"]?.AsArray() ?? [])
                .Select(parameter => parameter!["$ref"] is { } reference ? spec["components"]!["parameters"]![RefName(reference.GetValue<string>())]!.AsObject() : parameter.AsObject())
                .Where(parameter => parameter["in"]!.GetValue<string>() != "path")
                .ToList();
            if (parameters.Count == 0)
            {
                continue;
            }

            var name = string.Concat(operationId.Split('-').Select(Pascal)) + "Params";
            Write(Path.Combine(sdk, "Requests", name + ".cs"), ParamsFile(operationId, name, parameters));
            written.Add($"Requests/{name}.cs");
        }

        return written;
    }

    private IEnumerable<(string OperationId, JsonObject Operation)> Operations()
    {
        foreach (var (_, item) in spec["paths"]!.AsObject())
        {
            foreach (var (method, operation) in item!.AsObject())
            {
                if (Methods.Contains(method))
                {
                    yield return (operation!["operationId"]!.GetValue<string>(), operation.AsObject());
                }
            }
        }
    }

    private static string Pascal(string value) => char.ToUpperInvariant(value[0]) + value[1..];

    private static string RefName(string reference) => reference[(reference.LastIndexOf('/') + 1)..];

    private static string ClassName(string schemaKey)
    {
        if (Renames.TryGetValue(schemaKey, out var renamed) || Handwritten.TryGetValue(schemaKey, out renamed))
        {
            return renamed;
        }

        var segments = schemaKey.Split('.').AsEnumerable();
        if (Groups.Contains(segments.First()))
        {
            segments = segments.Skip(1);
        }

        return string.Concat(segments.Select(Pascal));
    }

    private static (bool IsRef, string Ref, JsonObject Inline) Unwrap(JsonObject property)
    {
        if (property["$ref"] is { } reference)
        {
            return (true, RefName(reference.GetValue<string>()), property);
        }

        if (property["allOf"] is JsonArray allOf)
        {
            return allOf.Count == 1 ? (true, RefName(allOf[0]!["$ref"]!.GetValue<string>()), property) : throw new InvalidOperationException("An allOf with more than one schema is not supported.");
        }

        return (false, "", property);
    }

    private void AddEnum(string name, JsonObject schema, string? description = null)
    {
        var values = schema["enum"]!.AsArray().Select(value => value!.ToJsonString().Trim('"')).ToList();
        if (_enums.TryGetValue(name, out var existing))
        {
            if (!existing.Values.SequenceEqual(values))
            {
                throw new InvalidOperationException($"The enum {name} is declared twice with different values.");
            }

            return;
        }

        List<string>? names = null;
        List<string>? descriptions = null;
        if (schema["x-enum-descriptions"] is JsonArray xd && xd.Count > 0)
        {
            if (xd[0] is JsonValue)
            {
                names = xd.Select(d => d!.GetValue<string>().Split(':', 2)[1].Trim().Split(' ')[0]).ToList();
                descriptions = xd.Select(d => d!.GetValue<string>().Split(':', 2)[1].Trim()).ToList();
            }
            else
            {
                var byValue = xd.SelectMany(d => d!.AsObject()).ToDictionary(entry => entry.Key, entry => entry.Value!.GetValue<string>());
                descriptions = values.Select(value => byValue[value]).ToList();
            }
        }

        _enums[name] = new EnumDefinition(values, description ?? schema["description"]?.GetValue<string>(), schema["type"]?.GetValue<string>() == "integer", names, descriptions);
    }

    private void AddClass(string name, string schemaKey, JsonObject schema)
    {
        if (_classes.ContainsKey(name))
        {
            return;
        }

        var properties = schema["properties"]?.AsObject().Select(entry => (entry.Key, entry.Value!.AsObject())).ToList() ?? [];
        _classes[name] = new ClassDefinition(schemaKey, schema["description"]?.GetValue<string>(), properties);
        foreach (var (jsonName, property) in properties)
        {
            VisitProperty(name, schemaKey, jsonName, property);
        }
    }

    private void VisitProperty(string className, string schemaKey, string jsonName, JsonObject property)
    {
        var (isRef, reference, detail) = Unwrap(property);
        if (isRef)
        {
            VisitRef(reference);
        }
        else if (detail["enum"] is not null)
        {
            var key = $"{schemaKey}.{jsonName}";
            AddEnum(InlineEnums.TryGetValue(key, out var name) ? name : throw new InvalidOperationException($"The inline enum {key} needs a name in InlineEnums."), detail);
        }
        else if (Type(detail) == "array")
        {
            VisitProperty(className, schemaKey, jsonName, detail["items"]!.AsObject());
        }
        else if (Type(detail) == "object" && detail["properties"] is not null)
        {
            AddClass(className + Pascal(jsonName), $"{schemaKey}.{jsonName}", detail);
        }
    }

    private void VisitRef(string schemaKey)
    {
        if (Skip.Contains(schemaKey))
        {
            return;
        }

        var schema = _schemas[schemaKey]!.AsObject();
        if (schema["enum"] is not null)
        {
            AddEnum(ClassName(schemaKey), schema);
        }
        else
        {
            AddClass(ClassName(schemaKey), schemaKey, schema);
        }
    }

    private static string? Type(JsonObject schema) => schema["type"]?.GetValue<string>();

    // A class used only by request bodies goes in Requests; every other class goes in Models.
    private void AssignNamespaces()
    {
        var requestRoots = new List<string>();
        var responseRoots = new List<string> { "VolumesReport" };
        foreach (var (_, operation) in Operations())
        {
            if (operation["requestBody"] is { } body)
            {
                requestRoots.Add(ClassName(RefName(body["content"]!["application/json"]!["schema"]!["$ref"]!.GetValue<string>())));
            }

            foreach (var (code, response) in operation["responses"]!.AsObject())
            {
                if (!code.StartsWith('2') || response?["content"]?["application/json"]?["schema"] is not JsonObject schema)
                {
                    continue;
                }

                if (schema["$ref"] is { } reference)
                {
                    responseRoots.Add(ClassName(RefName(reference.GetValue<string>())));
                }
                else if (schema["properties"] is JsonObject properties)
                {
                    foreach (var (_, property) in properties)
                    {
                        var (isRef, name, detail) = Unwrap(property!.AsObject());
                        while (!isRef && Type(detail) == "array")
                        {
                            (isRef, name, detail) = Unwrap(detail["items"]!.AsObject());
                        }

                        if (isRef)
                        {
                            responseRoots.Add(ClassName(name));
                        }
                    }
                }
            }
        }

        var inRequest = Reach(requestRoots);
        var inResponse = Reach(responseRoots);
        foreach (var name in _classes.Keys)
        {
            if (!inRequest.Contains(name) && !inResponse.Contains(name))
            {
                throw new InvalidOperationException($"The class {name} is used by no operation.");
            }

            _namespaces[name] = inRequest.Contains(name) && !inResponse.Contains(name) ? "Request" : "Model";
        }
    }

    private HashSet<string> Reach(IEnumerable<string> roots)
    {
        var seen = new HashSet<string>();
        var stack = new Stack<string>(roots.Where(_classes.ContainsKey));
        while (stack.TryPop(out var name))
        {
            if (!seen.Add(name))
            {
                continue;
            }

            foreach (var (jsonName, property) in _classes[name].Properties)
            {
                var (isRef, reference, detail) = Unwrap(property);
                while (!isRef && Type(detail) == "array")
                {
                    (isRef, reference, detail) = Unwrap(detail["items"]!.AsObject());
                }

                var child = isRef && !Skip.Contains(reference) && _schemas[reference]!["enum"] is null ? ClassName(reference)
                    : !isRef && Type(detail) == "object" && detail["properties"] is not null ? name + Pascal(jsonName)
                    : null;
                if (child is not null && _classes.ContainsKey(child))
                {
                    stack.Push(child);
                }
            }
        }

        return seen;
    }

    private PropertyType TypeOf(string className, string schemaKey, string jsonName, JsonObject property)
    {
        var (isRef, reference, detail) = Unwrap(property);
        var nullable = property["nullable"]?.GetValue<bool>() == true;
        var q = nullable ? "?" : "";
        if (isRef)
        {
            if (reference == "Id")
            {
                return new("string?");
            }

            if (reference == "Metadata")
            {
                return new("Dictionary<string, string>?");
            }

            if (Handwritten.TryGetValue(reference, out var handwritten))
            {
                return new(handwritten + "?", Class: handwritten);
            }

            if (_schemas[reference]!["enum"] is not null)
            {
                var name = ClassName(reference);
                return new(_enums[name].IsInteger ? "int" + q : "string?", Enum: name);
            }

            return new(ClassName(reference) + "?", Class: ClassName(reference));
        }

        if (detail["enum"] is not null)
        {
            return new("string?", Enum: InlineEnums[$"{schemaKey}.{jsonName}"]);
        }

        switch (Type(detail))
        {
            case "array":
                var item = TypeOf(className, schemaKey, jsonName, detail["items"]!.AsObject());
                return item.Class is not null
                    ? new($"List<{item.Class}>?", Class: item.Class)
                    : new($"List<{item.Net.TrimEnd('?')}>?", ItemEnum: item.Enum);
            case "object":
                return detail["properties"] is not null
                    ? new(className + Pascal(jsonName) + "?", Class: className + Pascal(jsonName))
                    : new("Dictionary<string, JsonElement>?");
            case "string":
                return detail["format"]?.GetValue<string>() switch
                {
                    "date-time" => new("DateTimeOffset" + q),
                    "date" => new("DateOnly" + q),
                    _ => new("string?"),
                };
            case "integer":
                return new((detail["format"]?.GetValue<string>() == "int64" ? "long" : "int") + q);
            case "number":
                return new("double" + q);
            case "boolean":
                return new("bool" + q);
        }

        throw new InvalidOperationException($"The property {className}.{jsonName} has a type the generator does not know.");
    }

    private string ClassFile(string name, ClassDefinition definition)
    {
        var ns = _namespaces[name];
        var usings = new SortedSet<string>(StringComparer.Ordinal);
        var properties = new List<string>();
        foreach (var (jsonName, property) in definition.Properties)
        {
            var type = TypeOf(name, definition.SchemaKey, jsonName, property);
            var initializer = "";
            // A response may leave any property out, so only a class used by requests alone can refuse null.
            if (ns == "Request" && property["nullable"]?.GetValue<bool>() == false && type.Net.EndsWith('?'))
            {
                type = type with { Net = type.Net.TrimEnd('?') };
                initializer = " = null!;";
            }

            var propertyName = jsonName == "$type" ? "Type" : Pascal(jsonName);
            if (propertyName == name)
            {
                throw new InvalidOperationException($"The property {name}.{propertyName} has the name of its class.");
            }

            var (isRef, _, detail) = Unwrap(property);
            var lines = Sentences(property["description"]?.GetValue<string>() ?? (isRef ? null : detail["description"]?.GetValue<string>()));
            string? remarks = null;
            if (type.Enum is not null)
            {
                remarks = $"One of the <see cref=\"{type.Enum}\"/> values.";
                usings.Add("Voucherly.Sdk.Enums");
            }

            if (type.ItemEnum is not null)
            {
                remarks = $"Each item is one of the <see cref=\"{type.ItemEnum}\"/> values.";
                usings.Add("Voucherly.Sdk.Enums");
            }

            if (type.Class is not null && _namespaces.TryGetValue(type.Class, out var classNamespace) && classNamespace != ns)
            {
                usings.Add(classNamespace == "Model" ? "Voucherly.Sdk.Models" : "Voucherly.Sdk.Requests");
            }

            if (type.Net.Contains("JsonElement"))
            {
                usings.Add("System.Text.Json");
            }

            var attribute = "";
            if (jsonName == "$type")
            {
                attribute = "    [JsonPropertyName(\"$type\")]\n";
                usings.Add("System.Text.Json.Serialization");
            }

            var doc = Doc(lines, "    ", remarks);
            if (property["deprecated"]?.GetValue<bool>() == true)
            {
                var message = string.Join(" ", lines).Replace("`", "").Replace("\"", "\\\"");
                attribute += message.Length > 0 ? $"    [Obsolete(\"{message}\")]\n" : "    [Obsolete]\n";
            }

            properties.Add(doc + attribute + $"    public {type.Net} {propertyName} {{ get; set {{ field = value; MarkAssigned(); }} }}{initializer}\n");
        }

        var file = new StringBuilder();
        foreach (var @using in usings)
        {
            file.Append($"using {@using};\n");
        }

        file.Append(usings.Count > 0 ? "\n" : "")
            .Append($"namespace Voucherly.Sdk.{(ns == "Model" ? "Models" : "Requests")};\n\n")
            .Append(Doc(Sentences(definition.Description), ""))
            .Append($"public class {name} : VoucherlyObject\n{{\n")
            .Append(string.Join("\n", properties))
            .Append("}\n");
        return file.ToString();
    }

    private static string EnumFile(string name, EnumDefinition definition)
    {
        var constants = new List<string>();
        for (var i = 0; i < definition.Values.Count; i++)
        {
            var value = definition.Values[i];
            var constant = definition.Names?[i] ?? Pascal(value);
            var literal = definition.IsInteger ? value : $"\"{value}\"";
            var lines = definition.Descriptions is null ? [] : Sentences(definition.Descriptions[i].TrimEnd().EndsWith('.') ? definition.Descriptions[i] : definition.Descriptions[i].TrimEnd() + ".");
            constants.Add(Doc(lines, "    ") + $"    public const {(definition.IsInteger ? "int" : "string")} {constant} = {literal};\n");
        }

        return "namespace Voucherly.Sdk.Enums;\n\n" + Doc(Sentences(definition.Description), "") + $"public static class {name}\n{{\n" + string.Join("\n", constants) + "}\n";
    }

    private string ParamsFile(string operationId, string name, List<JsonObject> parameters)
    {
        var usings = new SortedSet<string>(StringComparer.Ordinal);
        var properties = new List<string>();
        foreach (var parameter in parameters)
        {
            var schema = parameter["schema"]!.AsObject();
            var parameterName = parameter["name"]!.GetValue<string>();
            var isHeader = parameter["in"]!.GetValue<string>() == "header";
            var propertyName = isHeader
                ? string.Concat(parameterName.Replace("Voucherly-", "").Split('-').Select(word => Pascal(word.ToLowerInvariant())))
                : Pascal(parameterName);
            string net;
            string? enumName = null;
            string? itemEnum = null;
            if (parameterName == "include")
            {
                net = "IReadOnlyList<string>?";
                itemEnum = IncludeEnums.TryGetValue(operationId, out var include) ? include : ClassName(RefName(schema["items"]!["$ref"]!.GetValue<string>()));
            }
            else if (schema["$ref"] is { } reference)
            {
                net = "string?";
                enumName = RefName(reference.GetValue<string>()) == "Id" ? null : ClassName(RefName(reference.GetValue<string>()));
            }
            else if (Type(schema) == "array")
            {
                net = "IReadOnlyList<string>?";
                var itemRef = schema["items"]!["$ref"]?.GetValue<string>();
                itemEnum = itemRef is not null && _schemas[RefName(itemRef)]!["enum"] is not null ? ClassName(RefName(itemRef)) : null;
            }
            else
            {
                net = (Type(schema), schema["format"]?.GetValue<string>()) switch
                {
                    ("string", "date-time") => "DateTimeOffset?",
                    ("string", "date") => "DateOnly?",
                    ("string", _) => "string?",
                    ("integer", _) => "int?",
                    ("boolean", _) => "bool?",
                    _ => throw new InvalidOperationException($"The parameter {parameterName} of {operationId} has a type the generator does not know."),
                };
            }

            string? remarks = null;
            if (enumName is not null)
            {
                remarks = $"One of the <see cref=\"{enumName}\"/> values.";
                usings.Add("Voucherly.Sdk.Enums");
            }

            if (itemEnum is not null)
            {
                remarks = $"Each item is one of the <see cref=\"{itemEnum}\"/> values.";
                usings.Add("Voucherly.Sdk.Enums");
            }

            var required = "";
            if (parameter["required"]?.GetValue<bool>() == true)
            {
                net = net.TrimEnd('?');
                required = "required ";
            }

            var attribute = isHeader ? $"    [HeaderParam(\"{parameterName}\")]\n" : "";
            properties.Add(Doc(Sentences(parameter["description"]?.GetValue<string>()), "    ", remarks) + attribute + $"    public {required}{net} {propertyName} {{ get; set; }}\n");
        }

        var file = new StringBuilder();
        foreach (var @using in usings)
        {
            file.Append($"using {@using};\n");
        }

        return file.Append(usings.Count > 0 ? "\n" : "")
            .Append("namespace Voucherly.Sdk.Requests;\n\n")
            .Append($"public sealed class {name} : RequestParams\n{{\n")
            .Append(string.Join("\n", properties))
            .Append("}\n")
            .ToString();
    }

    // Joins the lines of a description, keeping a line break only after a line that ends with a full stop and before a list item.
    private static List<string> Sentences(string? text)
    {
        var lines = new List<string>();
        if (string.IsNullOrWhiteSpace(text))
        {
            return lines;
        }

        var current = "";
        foreach (var raw in text.Trim().Split('\n'))
        {
            var line = raw.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            if (current.Length > 0 && (current.EndsWith('.') || line.StartsWith("- ")))
            {
                lines.Add(current);
                current = line;
            }
            else
            {
                current = current.Length > 0 ? current + " " + line : line;
            }
        }

        if (current.Length > 0)
        {
            lines.Add(current);
        }

        return lines;
    }

    private static string Doc(List<string> lines, string indent, string? remarks = null)
    {
        var doc = new StringBuilder();
        if (lines.Count > 0)
        {
            if (!".!?:".Contains(lines[0][^1]))
            {
                lines[0] += ".";
            }

            doc.Append(indent).Append("/// <summary>\n");
            foreach (var line in lines)
            {
                doc.Append(indent).Append("/// ").Append(Xml(line)).Append('\n');
            }

            doc.Append(indent).Append("/// </summary>\n");
        }

        if (remarks is not null)
        {
            doc.Append(indent).Append("/// <remarks>").Append(remarks).Append("</remarks>\n");
        }

        return doc.ToString();
    }

    private static string Xml(string text)
    {
        text = text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        text = Regex.Replace(text, @"\[([^\]]+)\]\(([^)]+)\)", "<see href=\"$2\">$1</see>");
        return Regex.Replace(text, "`([^`]+)`", "<c>$1</c>");
    }

    private static void Write(string path, string content)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    }

    private sealed record ClassDefinition(string SchemaKey, string? Description, List<(string Key, JsonObject Value)> Properties);

    private sealed record EnumDefinition(List<string> Values, string? Description, bool IsInteger, List<string>? Names, List<string>? Descriptions);

    private sealed record PropertyType(string Net, string? Enum = null, string? ItemEnum = null, string? Class = null);
}
