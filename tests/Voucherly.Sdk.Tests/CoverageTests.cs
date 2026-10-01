using System.Reflection;
using System.Text.Json.Nodes;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests;

public class CoverageTests
{
    [Fact]
    public void EveryOperationOfTheSpecIsMappedToAMethodOrExcludedWithAReason()
    {
        var operations = SpecExamples.Operations();
        var coverage = JsonNode.Parse(File.ReadAllText(SpecExamples.SpecFile("coverage.json")))!.AsObject();
        var mapped = coverage["operations"]!.AsObject().ToDictionary(entry => entry.Key, entry => entry.Value!.GetValue<string>());
        var excluded = coverage["excluded"]!.AsObject().ToDictionary(entry => entry.Key, entry => entry.Value?.GetValue<string>());
        var errors = new List<string>();

        foreach (var (id, route) in operations)
        {
            if (!mapped.ContainsKey(id) && !excluded.ContainsKey(id))
            {
                errors.Add($"{id} ({route}) is neither mapped nor excluded.");
            }
        }
        foreach (var id in mapped.Keys.Concat(excluded.Keys).Where(id => !operations.ContainsKey(id)))
        {
            errors.Add($"{id} is in coverage.json but no longer in the spec.");
        }
        foreach (var id in mapped.Keys.Intersect(excluded.Keys))
        {
            errors.Add($"{id} is both mapped and excluded.");
        }
        foreach (var (id, reason) in excluded.Where(entry => string.IsNullOrWhiteSpace(entry.Value)))
        {
            errors.Add($"{id} is excluded without a reason.");
        }
        foreach (var target in mapped.Values.GroupBy(target => target).Where(group => group.Count() > 1))
        {
            errors.Add($"{target.Key} is mapped to {target.Count()} operations.");
        }
        foreach (var (id, target) in mapped)
        {
            if (FindMethod(target) is { } error)
            {
                errors.Add($"{id} maps to {target}, but {error}.");
            }
        }

        errors.ShouldBeEmpty(string.Join(Environment.NewLine, errors));
    }

    private static string? FindMethod(string target)
    {
        var parts = target.Split('.');
        if (parts.Length != 2)
        {
            return "the target is not in the form Service.Method";
        }

        var service = typeof(IVoucherlyClient).GetProperty(parts[0], BindingFlags.Public | BindingFlags.Instance);
        if (service is null)
        {
            return $"{nameof(IVoucherlyClient)} has no property {parts[0]}";
        }

        return service.PropertyType.GetMethods().Any(method => method.Name == parts[1])
            ? null
            : $"{service.PropertyType.Name} has no method {parts[1]}";
    }
}
