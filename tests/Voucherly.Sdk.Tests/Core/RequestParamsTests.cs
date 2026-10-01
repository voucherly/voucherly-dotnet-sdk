using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Core;

public class RequestParamsTests
{
    [Fact]
    public void SendsNothingWhenNothingIsAssigned()
    {
        var parameters = new SampleParams();

        parameters.ToQueryString().ShouldBe("");
        parameters.ToHeaders().ShouldBeEmpty();
    }

    [Fact]
    public void FormatsEachTypeOfValue()
    {
        var parameters = new SampleParams
        {
            Name = "Bar & Pizzeria",
            IsActive = false,
            Include = ["Lines", "Transactions"],
            FromDate = new DateTimeOffset(2026, 9, 1, 0, 0, 0, TimeSpan.FromHours(2)),
            Date = new DateOnly(2026, 9, 30),
            Length = 100,
        };

        parameters.ToQueryString().ShouldBe("name=Bar%20%26%20Pizzeria&isActive=false&include=Lines&include=Transactions&fromDate=2026-09-01T00%3A00%3A00%2B02%3A00&date=2026-09-30&length=100");
    }

    [Fact]
    public void SendsHeaderParametersAsHeadersOnly()
    {
        var parameters = new SampleParams { WaitTime = 30 };

        parameters.ToQueryString().ShouldBe("");
        parameters.ToHeaders().ShouldBe([new KeyValuePair<string, string>("Voucherly-Wait-Time", "30")]);
    }
}
