using System.Net;
using System.Text.Json.Nodes;
using Voucherly.Sdk.Enums;
using Voucherly.Sdk.Exceptions;
using Voucherly.Sdk.Models;
using Voucherly.Sdk.Requests;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Services;

public class PaymentServiceTests : ServiceTestBase
{
    private const string PaymentId = "pay_01kg2gestgepdbmsn7hs6bsrwp";

    private static readonly byte[] Pdf = [0x25, 0x50, 0x44, 0x46, 0x2D, 0x31, 0x2E, 0x37, 0x0A, 0xE2, 0xE3];

    private static readonly Dictionary<string, string> PathId = new() { ["id"] = PaymentId };

    public static TheoryData<string> CreateExamples() => [.. SpecExamples.RequestExamples("create-payment").Keys];

    public static TheoryData<string> ConfirmExamples() => [.. SpecExamples.RequestExamples("confirm-payment").Keys];

    public static TheoryData<string> RefundExamples() => [.. SpecExamples.RequestExamples("refund-payment").Keys];

    [Theory]
    [MemberData(nameof(CreateExamples))]
    public async Task Create(string example)
    {
        var json = RespondWithSample("create-payment");
        var body = SpecExamples.RequestExamples("create-payment")[example];

        var payment = await Client.Payments.CreateAsync(Deserialize<CreatePaymentRequest>(body));

        AssertOperation("create-payment", body: body);
        AssertReadsEveryMember(json, payment);
        payment.Transactions.ShouldNotBeNull()[0].ShouldBeOfType<Transaction>();
    }

    [Fact]
    public async Task RetrieveWaitsWithTheWaitTimeHeader()
    {
        var json = RespondWithSample("retrieve-payment");

        var payment = await Client.Payments.RetrieveAsync(PaymentId, new RetrievePaymentParams { Include = [PaymentInclude.Lines, PaymentInclude.Transactions], WaitTime = 30 });

        var request = AssertOperation("retrieve-payment", PathId, "include=Lines&include=Transactions");
        request.Headers["Voucherly-Wait-Time"].ShouldBe("30");
        AssertReadsEveryMember(json, payment);
    }

    [Fact]
    public async Task RetrieveWithoutParametersSendsNoWaitTime()
    {
        RespondWithSample("retrieve-payment");

        await Client.Payments.RetrieveAsync(PaymentId);

        AssertOperation("retrieve-payment", PathId).Headers.ContainsKey("Voucherly-Wait-Time").ShouldBeFalse();
    }

    [Theory]
    [MemberData(nameof(ConfirmExamples))]
    public async Task Confirm(string example)
    {
        var json = RespondWithSample("confirm-payment");
        var body = SpecExamples.RequestExamples("confirm-payment")[example];

        var payment = await Client.Payments.ConfirmAsync(PaymentId, Deserialize<ConfirmPaymentRequest>(body));

        AssertOperation("confirm-payment", PathId, body: body);
        AssertReadsEveryMember(json, payment);
    }

    [Fact]
    public async Task ConfirmWithoutRequestSendsAnEmptyObject()
    {
        RespondWithSample("confirm-payment");

        await Client.Payments.ConfirmAsync(PaymentId);

        AssertOperation("confirm-payment", PathId, body: new JsonObject());
    }

    [Fact]
    public async Task ConfirmThatConflictsExposesThePaymentStatus()
    {
        Handler.Respond(HttpStatusCode.Conflict, """{"title":"This operation is already processed.","status":409,"code":"ALREADY_CONFIRMED","paymentStatus":"Confirmed"}""");

        var exception = await Should.ThrowAsync<ConflictException>(() => Client.Payments.ConfirmAsync(PaymentId));

        exception.ErrorCode.ShouldBe("ALREADY_CONFIRMED");
        exception.PaymentStatus.ShouldBe(PaymentStatus.Confirmed);
    }

    [Fact]
    public async Task ConfirmThatCannotBeProcessedExposesTheOperationsAlreadyDone()
    {
        Handler.Respond(HttpStatusCode.UnprocessableEntity, """{"title":"The Payment can't be confirmed at the requested amounts.","status":422,"code":"IMPOSSIBLE_REFUND","operations":[{"transactionId":"tr_1","paymentGatewayId":"STRIPE","amount":3000}]}""");

        var exception = await Should.ThrowAsync<UnprocessableEntityException>(() => Client.Payments.ConfirmAsync(PaymentId));

        exception.ErrorCode.ShouldBe("IMPOSSIBLE_REFUND");
        exception.Extensions["operations"][0].GetProperty("amount").GetInt32().ShouldBe(3000);
    }

    [Theory]
    [MemberData(nameof(RefundExamples))]
    public async Task Refund(string example)
    {
        var json = RespondWithSample("refund-payment");
        var body = SpecExamples.RequestExamples("refund-payment")[example];

        var payment = await Client.Payments.RefundAsync(PaymentId, Deserialize<RefundPaymentRequest>(body));

        AssertOperation("refund-payment", PathId, body: body);
        AssertReadsEveryMember(json, payment);
    }

    [Fact]
    public async Task RefundWithoutRequestSendsAnEmptyObject()
    {
        RespondWithSample("refund-payment");

        await Client.Payments.RefundAsync(PaymentId);

        AssertOperation("refund-payment", PathId, body: new JsonObject());
    }

    [Fact]
    public async Task Void()
    {
        var json = RespondWithSample("void-payment");

        var payment = await Client.Payments.VoidAsync(PaymentId);

        AssertOperation("void-payment", PathId, body: new JsonObject());
        AssertReadsEveryMember(json, payment);
    }

    [Fact]
    public async Task DownloadReceipt()
    {
        Handler.Respond(HttpStatusCode.OK, Pdf, "application/pdf");

        var pdf = await Client.Payments.DownloadReceiptAsync(PaymentId);

        AssertOperation("download-payment-receipt", PathId).Headers["Accept"].ShouldBe("application/pdf");
        pdf.ShouldBe(Pdf);
    }

    [Fact]
    public async Task DownloadRefundReceipt()
    {
        Handler.Respond(HttpStatusCode.OK, Pdf, "application/pdf");

        var pdf = await Client.Payments.DownloadRefundReceiptAsync(PaymentId);

        AssertOperation("download-payment-refund-receipt", PathId).Headers["Accept"].ShouldBe("application/pdf");
        pdf.ShouldBe(Pdf);
    }
}
