using System.Net;
using System.Text;

namespace Voucherly.Sdk.Tests.Support;

public sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>> _responses = new();

    public List<RecordedRequest> Requests { get; } = [];

    public RecordedRequest LastRequest => Requests.Count > 0 ? Requests[^1] : throw new InvalidOperationException("The handler received no request.");

    public FakeHttpMessageHandler Respond(HttpStatusCode status, string body = "", string contentType = "application/json", IDictionary<string, string>? headers = null) =>
        Respond(status, Encoding.UTF8.GetBytes(body), contentType, headers);

    public FakeHttpMessageHandler Respond(HttpStatusCode status, byte[] body, string contentType, IDictionary<string, string>? headers = null)
    {
        _responses.Enqueue((_, _) =>
        {
            var response = new HttpResponseMessage(status) { Content = new ByteArrayContent(body) };
            response.Content.Headers.TryAddWithoutValidation("Content-Type", contentType);
            foreach (var header in headers ?? new Dictionary<string, string>())
            {
                response.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return Task.FromResult(response);
        });
        return this;
    }

    public FakeHttpMessageHandler Fail(Exception exception)
    {
        _responses.Enqueue((_, _) => Task.FromException<HttpResponseMessage>(exception));
        return this;
    }

    public FakeHttpMessageHandler Hang()
    {
        _responses.Enqueue(async (_, cancellationToken) =>
        {
            await Task.Delay(Timeout.Infinite, cancellationToken);
            throw new InvalidOperationException("Unreachable.");
        });
        return this;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
        Requests.Add(new RecordedRequest(
            request.Method,
            request.RequestUri!,
            request.Headers.ToDictionary(header => header.Key, header => string.Join(", ", header.Value), StringComparer.OrdinalIgnoreCase),
            body,
            request.Content?.Headers.ContentType?.ToString()));

        if (!_responses.TryDequeue(out var respond))
        {
            throw new InvalidOperationException($"The handler has no response queued for {request.Method} {request.RequestUri}.");
        }

        return await respond(request, cancellationToken);
    }
}

public sealed record RecordedRequest(HttpMethod Method, Uri Uri, IReadOnlyDictionary<string, string> Headers, string? Body, string? ContentType);
