using System.Text.Json;

namespace Voucherly.Sdk.Exceptions;

/// <summary>
/// The API answered with a status outside 2xx.
/// </summary>
public class ApiException : VoucherlyException
{
    private static readonly HashSet<string> ProblemMembers = ["type", "title", "status", "detail", "code", "parameter"];

    private readonly Dictionary<string, JsonElement> _problem;

    public ApiException(int statusCode, string rawBody, IReadOnlyDictionary<string, string>? headers = null)
        : this(statusCode, rawBody, headers, ReadProblem(rawBody))
    {
    }

    private ApiException(int statusCode, string rawBody, IReadOnlyDictionary<string, string>? headers, Dictionary<string, JsonElement> problem)
        : base(BuildMessage(statusCode, problem))
    {
        StatusCode = statusCode;
        RawBody = rawBody;
        Headers = headers ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        _problem = problem;
        Extensions = problem.Where(member => !ProblemMembers.Contains(member.Key)).ToDictionary(member => member.Key, member => member.Value);
    }

    public int StatusCode { get; }

    /// <summary>
    /// A URI reference that identifies the problem type.
    /// </summary>
    public string? Type => ProblemString(_problem, "type");

    /// <summary>
    /// A short, human-readable summary of the problem type.
    /// </summary>
    public string? Title => ProblemString(_problem, "title");

    /// <summary>
    /// A human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    public string? Detail => ProblemString(_problem, "detail");

    /// <summary>
    /// A short string indicating the error, for the errors that can be handled programmatically.
    /// </summary>
    public string? ErrorCode => ProblemString(_problem, "code");

    /// <summary>
    /// The parameter the error relates to, when the error is parameter-specific.
    /// </summary>
    public string? Parameter => ProblemString(_problem, "parameter");

    /// <summary>
    /// The members of the problem details other than type, title, status, detail, code and parameter, such as <c>line</c> or <c>operations</c>.
    /// </summary>
    public IReadOnlyDictionary<string, JsonElement> Extensions { get; }

    public string RawBody { get; }

    /// <summary>
    /// The response headers, content headers included, with case-insensitive names.
    /// </summary>
    public IReadOnlyDictionary<string, string> Headers { get; }

    internal static ApiException Create(int statusCode, string rawBody, IReadOnlyDictionary<string, string> headers) => statusCode switch
    {
        400 => new BadRequestException(rawBody, headers),
        404 => new NotFoundException(rawBody, headers),
        409 => new ConflictException(rawBody, headers),
        422 => new UnprocessableEntityException(rawBody, headers),
        424 => new FailedDependencyException(rawBody, headers),
        _ => new ApiException(statusCode, rawBody, headers),
    };

    private static Dictionary<string, JsonElement> ReadProblem(string rawBody)
    {
        try
        {
            using var document = JsonDocument.Parse(rawBody);
            if (document.RootElement.ValueKind == JsonValueKind.Object)
            {
                return document.RootElement.EnumerateObject().ToDictionary(member => member.Name, member => member.Value.Clone());
            }
        }
        catch (JsonException)
        {
        }

        return [];
    }

    private static string BuildMessage(int statusCode, Dictionary<string, JsonElement> problem)
    {
        var message = $"{ProblemString(problem, "title")} {ProblemString(problem, "detail")}".Trim();
        return message.Length > 0 ? message : $"The Voucherly API answered with HTTP status {statusCode}.";
    }

    private static string? ProblemString(Dictionary<string, JsonElement> problem, string member) =>
        problem.TryGetValue(member, out var value) ? value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False => value.GetRawText(),
            _ => null,
        } : null;
}

/// <summary>
/// 400 Bad Request.
/// </summary>
public class BadRequestException(string rawBody, IReadOnlyDictionary<string, string>? headers = null) : ApiException(400, rawBody, headers);

/// <summary>
/// 404 Not Found.
/// </summary>
public class NotFoundException(string rawBody, IReadOnlyDictionary<string, string>? headers = null) : ApiException(404, rawBody, headers);

/// <summary>
/// 409 Conflict.
/// </summary>
public class ConflictException(string rawBody, IReadOnlyDictionary<string, string>? headers = null) : ApiException(409, rawBody, headers)
{
    /// <summary>
    /// The status of the Payment that prevented the operation, one of the <see cref="Enums.PaymentStatus"/> values.
    /// </summary>
    public string? PaymentStatus => Extensions.TryGetValue("paymentStatus", out var value) && value.ValueKind == JsonValueKind.String ? value.GetString() : null;
}

/// <summary>
/// 422 Unprocessable Entity.
/// </summary>
public class UnprocessableEntityException(string rawBody, IReadOnlyDictionary<string, string>? headers = null) : ApiException(422, rawBody, headers);

/// <summary>
/// 424 Failed Dependency.
/// </summary>
public class FailedDependencyException(string rawBody, IReadOnlyDictionary<string, string>? headers = null) : ApiException(424, rawBody, headers);
