using System.Text.Json;
using Voucherly.Sdk.Http;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Core;

public class VoucherlyObjectTests
{
    [Fact]
    public void SerializesOnlyAssignedPropertiesIncludingExplicitNull()
    {
        var sample = new SampleObject { Name = "Mario", Quantity = null };

        Serialize(sample).ShouldBe("""{"name":"Mario","quantity":null}""");
    }

    [Fact]
    public void SerializesAnObjectWithNothingAssignedAsAnEmptyJsonObject()
    {
        Serialize(new SampleObject()).ShouldBe("{}");
    }

    [Fact]
    public void SerializesDatesNestedObjectsAndRenamedProperties()
    {
        var child = new SampleChild { Code = "KG" };
        var sample = new SampleObject
        {
            Metadata = [],
            Day = new DateOnly(2026, 9, 30),
            Moment = new DateTimeOffset(2026, 9, 30, 15, 0, 0, TimeSpan.FromHours(2)),
            Child = child,
            Children = [child],
            Kind = "stripe",
        };

        Serialize(sample).ShouldBe("""{"metadata":{},"day":"2026-09-30","moment":"2026-09-30T15:00:00+02:00","child":{"code":"KG"},"children":[{"code":"KG"}],"$type":"stripe"}""");
    }

    [Fact]
    public void DeserializesNestedObjectsListsAndRenamedProperties()
    {
        var sample = Deserialize("""{"child":{"code":"KG","size":500},"children":[{"code":"G"},{"code":"L"}],"metadata":{"order":"42"},"$type":"satispay","rate":22.0}""");

        sample.Child.ShouldNotBeNull().Size.ShouldBe(500);
        sample.Children.ShouldNotBeNull().Count.ShouldBe(2);
        sample.Children[1].Code.ShouldBe("L");
        sample.Children[1].Size.ShouldBeNull();
        sample.Metadata.ShouldNotBeNull()["order"].ShouldBe("42");
        sample.Kind.ShouldBe("satispay");
        sample.Rate.ShouldBe(22.0);
    }

    [Theory]
    [InlineData("2026-09-08T01:24:43.0446376", "2026-09-08T01:24:43.0446376+00:00")]
    [InlineData("0001-01-01T00:00:00.0000000", "0001-01-01T00:00:00.0000000+00:00")]
    [InlineData("2026-09-08T01:24:43Z", "2026-09-08T01:24:43.0000000+00:00")]
    [InlineData("2026-09-08T03:24:43+02:00", "2026-09-08T03:24:43.0000000+02:00")]
    public void ReadsADateTimeWithoutOffsetAsUtc(string json, string expected)
    {
        var sample = Deserialize($$"""{"moment":"{{json}}"}""");

        sample.Moment.ShouldNotBeNull().ToString("O", System.Globalization.CultureInfo.InvariantCulture).ShouldBe(expected);
    }

    [Fact]
    public void KeepsTheUnknownMembersAsExtensionData()
    {
        var sample = Deserialize("""{"name":"Mario","newField":{"a":1},"child":{"code":"KG","unit":"g"}}""");

        sample.ExtensionData.ShouldNotBeNull().Keys.ShouldBe(["newField"]);
        sample.ExtensionData["newField"].GetRawText().ShouldBe("""{"a":1}""");
        sample.Child.ShouldNotBeNull().ExtensionData.ShouldNotBeNull()["unit"].GetString().ShouldBe("g");
    }

    [Fact]
    public void RoundTripsTheMembersThatWerePresent()
    {
        const string json = """{"name":"Mario","quantity":null,"day":"2026-09-30","child":{"code":"KG"},"children":[{"size":1}]}""";

        Serialize(Deserialize(json)).ShouldBe(json);
    }

    private static string Serialize(VoucherlyObject value) => JsonSerializer.Serialize(value, value.GetType(), VoucherlyJson.Options);

    private static SampleObject Deserialize(string json) => JsonSerializer.Deserialize<SampleObject>(json, VoucherlyJson.Options).ShouldNotBeNull();
}
