# Changelog

## 2.0.0 - 2026-10-01

Rewritten from the OpenAPI spec of the Voucherly API, as published on 2026-10-01. The upgrade from 1.x is described in [UPGRADE-2.0.md](https://github.com/voucherly/voucherly-dotnet-sdk/blob/main/UPGRADE-2.0.md).

- Every public operation of the API: 43 operations over companies, customers and their addresses, wallet and prepaid balance, payment methods, payments, receipts, terminals, stores, concept stores, store areas, payment gateways and the volumes report.
- `IVoucherlyClient`, with one service per tag, replaces `IVoucherlyApiService`. Every method is async, with a `CancellationToken`.
- `AddVoucherly` registers a typed `HttpClient` and returns its `IHttpClientBuilder`; without dependency injection, `new VoucherlyClient(options, httpClient)` takes your own client. The SDK never changes `BaseAddress` or `DefaultRequestHeaders`.
- A request sends only the properties assigned to it, an explicit `null` included. A request property that the API never accepts as `null` is not nullable.
- `PaymentMethods.ListAsync` pages like every other list: 10 PaymentMethods by default, up to 100 with `ListCustomerPaymentMethodParams.Length`.
- The fields the API deprecates are `[Obsolete]`: `PaymentLine.TotalAmount`, `TotalDiscountAmount` and `IsFood`.
- Responses accept fields and enum values that the SDK does not know yet: enums are `const string` values, and unknown fields go to `ExtensionData`.
- A date-time without an offset is read as UTC.
- Exceptions carry the HTTP status, the problem details and the raw body, with a subclass for each error status of the spec, and a `ConnectionException` for network errors and timeouts.
- `Voucherly-Wait-Time` is sent: 1.x prepared it and never passed it to the call.
- Platform keys, with the `MerchantId` and `Tenant` options.
- .NET 8 and .NET 10. The dependencies on `fbognini.Sdk` and `FastIDs.TypeId` are gone.

## 1.x

See the [releases on GitHub](https://github.com/voucherly/voucherly-dotnet-sdk/releases).
