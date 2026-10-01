using Voucherly.Sdk.Services;

namespace Voucherly.Sdk;

public interface IVoucherlyClient
{
    ICompanyService Companies { get; }

    ICustomerService Customers { get; }

    IPaymentMethodService PaymentMethods { get; }

    IPaymentService Payments { get; }

    IPaymentGatewayService PaymentGateways { get; }

    IReceiptService Receipts { get; }

    ITerminalService Terminals { get; }

    IStoreService Stores { get; }

    IConceptStoreService ConceptStores { get; }

    IStoreAreaService StoreAreas { get; }

    IReportService Reports { get; }
}
