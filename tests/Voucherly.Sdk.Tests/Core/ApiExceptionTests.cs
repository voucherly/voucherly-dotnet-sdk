using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Voucherly.Sdk.Exceptions;
using Voucherly.Sdk.Tests.Support;

namespace Voucherly.Sdk.Tests.Core;

public class ApiExceptionTests
{
    private static readonly Dictionary<int, Type> Types = new()
    {
        [400] = typeof(BadRequestException),
        [404] = typeof(NotFoundException),
        [409] = typeof(ConflictException),
        [422] = typeof(UnprocessableEntityException),
        [424] = typeof(FailedDependencyException),
    };

    private static readonly string[] ProblemMembers = ["type", "title", "status", "detail", "code", "parameter"];

    public static TheoryData<string, int, string> ErrorExamples()
    {
        var data = new TheoryData<string, int, string>();
        foreach (var (operationId, status, name, _) in SpecExamples.ErrorExamples())
        {
            data.Add(operationId, status, name);
        }

        return data;
    }

    [Fact]
    public void ThereIsAnExceptionForEveryErrorStatusOfTheSpec()
    {
        SpecExamples.ErrorExamples().Select(example => example.Status).Distinct().Order().ShouldBe(Types.Keys.Order());
    }

    [Theory]
    [MemberData(nameof(ErrorExamples))]
    public async Task ReadsTheProblemDetailsOfEveryErrorExampleOfTheSpec(string operationId, int status, string name)
    {
        var body = SpecExamples.ErrorExamples().Single(example => example.OperationId == operationId && example.Status == status && example.Name == name).Body.AsObject();
        var rawBody = body.ToJsonString();

        var exception = await ExceptionFor(status, rawBody, "application/problem+json");

        exception.GetType().ShouldBe(Types[status]);
        exception.StatusCode.ShouldBe(status);
        exception.Title.ShouldBe(body["title"]?.GetValue<string>());
        exception.Detail.ShouldBe(body["detail"]?.GetValue<string>());
        exception.ErrorCode.ShouldBe(body["code"]?.GetValue<string>());
        exception.Parameter.ShouldBe(body["parameter"]?.GetValue<string>());
        exception.Type.ShouldBe(body["type"]?.GetValue<string>());
        exception.RawBody.ShouldBe(rawBody);
        exception.Headers["content-type"].ShouldBe("application/problem+json");

        var extensions = body.Where(member => !ProblemMembers.Contains(member.Key)).ToList();
        exception.Extensions.Count.ShouldBe(extensions.Count);
        foreach (var (key, value) in extensions)
        {
            JsonNode.DeepEquals(JsonNode.Parse(exception.Extensions[key].GetRawText()), value).ShouldBeTrue(key);
        }
    }

    [Fact]
    public async Task BuildsTheMessageFromTitleAndDetail()
    {
        var exception = await ExceptionFor(404, """{"title":"Customer was not found.","detail":"Entity \"Customer\" (cs_1) was not found."}""");

        exception.Message.ShouldBe("Customer was not found. Entity \"Customer\" (cs_1) was not found.");
    }

    [Fact]
    public async Task ExposesThePaymentStatusOfAConflict()
    {
        var exception = await ExceptionFor(409, """{"title":"This operation is already processed.","status":409,"code":"ALREADY_CONFIRMED","paymentStatus":"Confirmed"}""");

        exception.ShouldBeOfType<ConflictException>().PaymentStatus.ShouldBe("Confirmed");
    }

    [Fact]
    public async Task UsesTheBaseExceptionForTheStatusesTheSpecDoesNotDeclare()
    {
        var exception = await ExceptionFor(401, """{"title":"Unauthorized","status":401}""");

        exception.GetType().ShouldBe(typeof(ApiException));
        exception.StatusCode.ShouldBe(401);
    }

    [Fact]
    public async Task KeepsTheRawBodyWhenItIsNotJson()
    {
        var exception = await ExceptionFor(502, "<html>Bad Gateway</html>", "text/html");

        exception.Message.ShouldBe("The Voucherly API answered with HTTP status 502.");
        exception.RawBody.ShouldBe("<html>Bad Gateway</html>");
        exception.Title.ShouldBeNull();
        exception.Extensions.ShouldBeEmpty();
    }

    private static async Task<ApiException> ExceptionFor(int status, string body, string contentType = "application/json")
    {
        var (service, handler) = TestClients.Create();
        handler.Respond((HttpStatusCode)status, body, contentType);

        return await Should.ThrowAsync<ApiException>(() => service.JsonAsync<JsonElement>(HttpMethod.Get, "/v1/things"));
    }
}
