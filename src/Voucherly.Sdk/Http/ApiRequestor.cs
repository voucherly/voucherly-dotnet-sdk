using System.Net.Http.Headers;
using System.Text.Json;
using Voucherly.Sdk.Exceptions;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk.Http;

internal sealed class ApiRequestor
{
    private static readonly MediaTypeHeaderValue JsonContentType = new("application/json") { CharSet = "utf-8" };

    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly KeyValuePair<string, string>[] _headers;

    public ApiRequestor(HttpClient httpClient, VoucherlyClientOptions options)
    {
        _httpClient = httpClient;
        _baseUrl = options.BaseUrl.AbsoluteUri.TrimEnd('/');

        var headers = new List<KeyValuePair<string, string>>
        {
            new("Voucherly-API-Key", options.ApiKey),
            new("User-Agent", $"VoucherlyApiDotnetSdk/{VoucherlyClient.Version}"),
        };
        AddIfNotEmpty(headers, "Voucherly-Merchant-Id", options.MerchantId);
        AddIfNotEmpty(headers, "Voucherly-Tenant", options.Tenant);
        AddIfNotEmpty(headers, "x-voucherly-os", options.Os);
        AddIfNotEmpty(headers, "x-voucherly-osversion", options.OsVersion);
        AddIfNotEmpty(headers, "x-voucherly-osframework", options.OsFramework);
        AddIfNotEmpty(headers, "x-voucherly-app", options.App);
        AddIfNotEmpty(headers, "x-voucherly-appversion", options.AppVersion);
        AddIfNotEmpty(headers, "x-voucherly-apphouse", options.AppHouse);
        AddIfNotEmpty(headers, "x-voucherly-devicetype", options.DeviceType);
        _headers = [.. headers];
    }

    public async Task<T> SendAsync<T>(HttpMethod method, string path, VoucherlyObject? body, RequestParams? parameters, CancellationToken cancellationToken)
    {
        using var response = await SendRawAsync(method, path, body, parameters, "application/json", cancellationToken).ConfigureAwait(false);
        var content = await ReadAsync(response.Content.ReadAsByteArrayAsync, cancellationToken).ConfigureAwait(false);

        try
        {
            return JsonSerializer.Deserialize<T>(content, VoucherlyJson.Options) ?? throw new VoucherlyException("The response body is empty.");
        }
        catch (JsonException exception)
        {
            throw new VoucherlyException($"The response body is not valid JSON: {exception.Message}", exception);
        }
    }

    public async Task SendNoContentAsync(HttpMethod method, string path, RequestParams? parameters, CancellationToken cancellationToken)
    {
        using var response = await SendRawAsync(method, path, null, parameters, "application/json", cancellationToken).ConfigureAwait(false);
    }

    public async Task<byte[]> SendBytesAsync(string path, string accept, CancellationToken cancellationToken)
    {
        using var response = await SendRawAsync(HttpMethod.Get, path, null, null, accept, cancellationToken).ConfigureAwait(false);
        return await ReadAsync(response.Content.ReadAsByteArrayAsync, cancellationToken).ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> SendRawAsync(HttpMethod method, string path, VoucherlyObject? body, RequestParams? parameters, string accept, CancellationToken cancellationToken)
    {
        var url = _baseUrl + path;
        var query = parameters?.ToQueryString();
        if (!string.IsNullOrEmpty(query))
        {
            url += "?" + query;
        }

        using var request = new HttpRequestMessage(method, new Uri(url, UriKind.Absolute));
        foreach (var header in _headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
        foreach (var header in parameters?.ToHeaders() ?? [])
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
        request.Headers.Accept.ParseAdd(accept);

        if (body is not null)
        {
            request.Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(body, body.GetType(), VoucherlyJson.Options));
            request.Content.Headers.ContentType = JsonContentType;
        }
        else if (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch)
        {
            // The API answers 415 to a POST without a JSON body, even where the spec declares none.
            request.Content = new ByteArrayContent("{}"u8.ToArray());
            request.Content.Headers.ContentType = JsonContentType;
        }

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException exception)
        {
            throw new ConnectionException(exception.Message, exception);
        }
        catch (TaskCanceledException exception) when (!cancellationToken.IsCancellationRequested)
        {
            throw new ConnectionException("The request to the Voucherly API timed out.", exception);
        }

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        using (response)
        {
            var rawBody = await ReadAsync(response.Content.ReadAsStringAsync, cancellationToken).ConfigureAwait(false);
            throw ApiException.Create((int)response.StatusCode, rawBody, ReadHeaders(response));
        }
    }

    private static async Task<T> ReadAsync<T>(Func<CancellationToken, Task<T>> read, CancellationToken cancellationToken)
    {
        try
        {
            return await read(cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException exception)
        {
            throw new ConnectionException(exception.Message, exception);
        }
        catch (IOException exception)
        {
            throw new ConnectionException(exception.Message, exception);
        }
    }

    private static Dictionary<string, string> ReadHeaders(HttpResponseMessage response)
    {
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var header in response.Headers.Concat(response.Content.Headers))
        {
            headers[header.Key] = string.Join(", ", header.Value);
        }

        return headers;
    }

    private static void AddIfNotEmpty(List<KeyValuePair<string, string>> headers, string name, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            headers.Add(new(name, value));
        }
    }
}
