using System.Globalization;
using Voucherly.Sdk.Http;

namespace Voucherly.Sdk.Services;

internal abstract class ServiceBase(ApiRequestor requestor)
{
    protected ApiRequestor Requestor { get; } = requestor;

    /// <summary>
    /// Fills the placeholders of <paramref name="format"/> with the ids, each encoded as a path segment.
    /// </summary>
    protected internal static string Path(string format, params string[] ids)
    {
        foreach (var id in ids)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("An id cannot be null or empty.", nameof(ids));
            }
        }

        return string.Format(CultureInfo.InvariantCulture, format, ids.Select(Uri.EscapeDataString).ToArray<object>());
    }
}
