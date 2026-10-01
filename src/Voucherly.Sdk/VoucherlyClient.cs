using System.Net;
using System.Reflection;
using Voucherly.Sdk.Http;
using Voucherly.Sdk.Services;

namespace Voucherly.Sdk;

public sealed class VoucherlyClient : IVoucherlyClient
{
    private static readonly SocketsHttpHandler SharedHandler = new()
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        ConnectTimeout = TimeSpan.FromSeconds(10),
        AutomaticDecompression = DecompressionMethods.All,
    };

    /// <param name="options">The key and the headers sent with every request.</param>
    /// <param name="httpClient">The client that sends the requests. The SDK never changes its <c>BaseAddress</c> or <c>DefaultRequestHeaders</c>, so it can be shared. Without it, the SDK uses a client of its own with a 30 seconds timeout.</param>
    public VoucherlyClient(VoucherlyClientOptions options, HttpClient? httpClient = null)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            throw new ArgumentException("The ApiKey option is required.", nameof(options));
        }

        Requestor = new ApiRequestor(httpClient ?? new HttpClient(SharedHandler, disposeHandler: false) { Timeout = TimeSpan.FromSeconds(30) }, options);
        Companies = new CompanyService(Requestor);
        Customers = new CustomerService(Requestor);
        PaymentMethods = new PaymentMethodService(Requestor);
        Payments = new PaymentService(Requestor);
        PaymentGateways = new PaymentGatewayService(Requestor);
        Receipts = new ReceiptService(Requestor);
        Terminals = new TerminalService(Requestor);
        Stores = new StoreService(Requestor);
        ConceptStores = new ConceptStoreService(Requestor);
        StoreAreas = new StoreAreaService(Requestor);
        Reports = new ReportService(Requestor);
    }

    /// <summary>
    /// The version of the SDK, sent in the User-Agent header.
    /// </summary>
    public static string Version { get; } = typeof(VoucherlyClient).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] ?? "0.0.0";

    public ICompanyService Companies { get; }

    public ICustomerService Customers { get; }

    public IPaymentMethodService PaymentMethods { get; }

    public IPaymentService Payments { get; }

    public IPaymentGatewayService PaymentGateways { get; }

    public IReceiptService Receipts { get; }

    public ITerminalService Terminals { get; }

    public IStoreService Stores { get; }

    public IConceptStoreService ConceptStores { get; }

    public IStoreAreaService StoreAreas { get; }

    public IReportService Reports { get; }

    internal ApiRequestor Requestor { get; }
}
