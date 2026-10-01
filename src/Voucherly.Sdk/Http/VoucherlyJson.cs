using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;

namespace Voucherly.Sdk.Http;

internal static class VoucherlyJson
{
    public static JsonSerializerOptions Options { get; } = CreateOptions();

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            TypeInfoResolver = new DefaultJsonTypeInfoResolver { Modifiers = { SerializeOnlyAssignedProperties } },
            Converters = { new UtcDateTimeOffsetConverter() },
        };
        options.MakeReadOnly();
        return options;
    }

    private static void SerializeOnlyAssignedProperties(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object || !typeof(VoucherlyObject).IsAssignableFrom(typeInfo.Type))
        {
            return;
        }

        foreach (var property in typeInfo.Properties)
        {
            if (property.IsExtensionData || property.AttributeProvider is not PropertyInfo clrProperty)
            {
                continue;
            }

            var name = clrProperty.Name;
            property.ShouldSerialize = (target, _) => ((VoucherlyObject)target).IsAssigned(name);
        }
    }
}
