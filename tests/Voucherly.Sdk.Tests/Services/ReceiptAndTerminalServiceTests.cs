using System.Net;
using Voucherly.Sdk.Enums;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Services;

public class ReceiptAndTerminalServiceTests : ServiceTestBase
{
    private static readonly byte[] Pdf = [0x25, 0x50, 0x44, 0x46, 0x2D, 0x31, 0x2E, 0x37, 0x0A, 0xE2, 0xE3];

    [Fact]
    public async Task RetrieveReceipt()
    {
        var json = RespondWithSample("retrieve-receipt");

        var receipt = await Client.Receipts.RetrieveAsync("rcp_1");

        AssertOperation("retrieve-receipt", new Dictionary<string, string> { ["id"] = "rcp_1" });
        AssertReadsEveryMember(json, receipt);
    }

    [Fact]
    public async Task DownloadReceipt()
    {
        Handler.Respond(HttpStatusCode.OK, Pdf, "application/pdf");

        var pdf = await Client.Receipts.DownloadAsync("rcp_1");

        AssertOperation("download-receipt", new Dictionary<string, string> { ["id"] = "rcp_1" }).Headers["Accept"].ShouldBe("application/pdf");
        pdf.ShouldBe(Pdf);
    }

    [Fact]
    public async Task ListTerminals()
    {
        var json = RespondWithSample("list-terminal");

        var page = await Client.Terminals.ListAsync(new ListTerminalParams
        {
            PaymentGatewayAccountId = "pga_1",
            PaymentGatewayId = "NEXI",
            StoreId = "sto_1",
            Status = TerminalStatus.Active,
            Length = 50,
        });

        AssertOperation("list-terminal", query: "paymentGatewayAccountId=pga_1&paymentGatewayId=NEXI&storeId=sto_1&status=Active&length=50");
        page.Items[0].ShouldBeOfType<Terminal>();
        AssertReadsEveryMember(json, page);
    }

    [Fact]
    public async Task DeleteTerminal()
    {
        RespondWithSample("delete-terminal");

        await Client.Terminals.DeleteAsync("ter_1");

        AssertOperation("delete-terminal", new Dictionary<string, string> { ["id"] = "ter_1" });
    }
}
