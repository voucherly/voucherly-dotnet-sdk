using Voucherly.Sdk.Enums;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Services;

public class PaymentGatewayAndReportServiceTests : ServiceTestBase
{
    [Fact]
    public async Task ListPaymentGateways()
    {
        var json = RespondWithSample("list-payment-gateway");

        var list = await Client.PaymentGateways.ListAsync(new ListPaymentGatewayParams { StoreId = "sto_1", All = true, Include = [PaymentGatewayInclude.Parameters] });

        AssertOperation("list-payment-gateway", query: "storeId=sto_1&all=true&include=Parameters");
        list.Items.ShouldNotBeNull()[0].ShouldBeOfType<PaymentGateway>();
        AssertReadsEveryMember(json, list);
    }

    [Fact]
    public async Task ListPaymentGatewaysWithoutParameters()
    {
        RespondWithSample("list-payment-gateway");

        await Client.PaymentGateways.ListAsync();

        AssertOperation("list-payment-gateway");
    }

    [Fact]
    public async Task Volumes()
    {
        var json = RespondWithSample("volumes-report");

        var report = await Client.Reports.VolumesAsync(new VolumesReportParams
        {
            FromDate = new DateTimeOffset(2026, 9, 1, 0, 0, 0, TimeSpan.Zero),
            ToDate = new DateTimeOffset(2026, 9, 30, 0, 0, 0, TimeSpan.Zero),
            PaymentMode = PaymentMode.Payment,
            GroupBy = [RevenuesGrouping.Store, RevenuesGrouping.Company],
            PaymentGatewayIds = ["SATISPAY", "EDENRED"],
        });

        AssertOperation("volumes-report", query: "fromDate=2026-09-01T00%3A00%3A00%2B00%3A00&toDate=2026-09-30T00%3A00%3A00%2B00%3A00&paymentMode=Payment&groupBy=Store&groupBy=Company&paymentGatewayIds=SATISPAY&paymentGatewayIds=EDENRED");
        report.Items.ShouldNotBeNull()[0].ShouldBeOfType<RevenuesDetails>();
        AssertReadsEveryMember(json, report);
    }
}
