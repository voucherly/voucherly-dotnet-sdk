using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Voucherly.Sdk.Requests;

/// <summary>
/// Base of the query and header parameters of an operation.
/// Only the properties whose value is not null are sent.
/// </summary>
public abstract class RequestParams
{
    internal string ToQueryString()
    {
        var query = new StringBuilder();
        foreach (var (property, value) in AssignedValues())
        {
            if (property.GetCustomAttribute<HeaderParamAttribute>() is not null)
            {
                continue;
            }

            var name = Uri.EscapeDataString(JsonNamingPolicy.CamelCase.ConvertName(property.Name));
            IEnumerable values = value is IEnumerable enumerable and not string ? enumerable : new[] { value };
            foreach (var item in values)
            {
                query.Append(query.Length == 0 ? "" : "&").Append(name).Append('=').Append(Uri.EscapeDataString(Format(item)));
            }
        }

        return query.ToString();
    }

    internal IEnumerable<KeyValuePair<string, string>> ToHeaders()
    {
        foreach (var (property, value) in AssignedValues())
        {
            if (property.GetCustomAttribute<HeaderParamAttribute>() is { } header)
            {
                yield return new(header.Name, Format(value));
            }
        }
    }

    private IEnumerable<(PropertyInfo Property, object Value)> AssignedValues()
    {
        foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.GetValue(this) is { } value)
            {
                yield return (property, value);
            }
        }
    }

    private static string Format(object? value) => value switch
    {
        null => "",
        bool boolean => boolean ? "true" : "false",
        DateOnly date => date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        DateTimeOffset dateTime => dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.FFFFFFFzzz", CultureInfo.InvariantCulture),
        IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
        _ => value.ToString() ?? "",
    };
}

[AttributeUsage(AttributeTargets.Property)]
internal sealed class HeaderParamAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
