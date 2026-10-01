using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Voucherly.Sdk;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IVoucherlyClient"/> as a typed HttpClient.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Configures the options. Leave it null when they are configured elsewhere, for example bound from configuration.</param>
    /// <returns>The builder of the HttpClient, to add your own handlers to it.</returns>
    public static IHttpClientBuilder AddVoucherly(this IServiceCollection services, Action<VoucherlyClientOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        var options = services.AddOptions<VoucherlyClientOptions>();
        if (configure is not null)
        {
            options.Configure(configure);
        }

        return services
            .AddHttpClient<IVoucherlyClient, VoucherlyClient>((httpClient, provider) => new VoucherlyClient(provider.GetRequiredService<IOptions<VoucherlyClientOptions>>().Value, httpClient))
            .ConfigureHttpClient(httpClient => httpClient.Timeout = TimeSpan.FromSeconds(30));
    }
}
