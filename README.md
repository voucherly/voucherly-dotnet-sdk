# Voucherly .NET SDK

[![NuGet](https://img.shields.io/nuget/v/voucherly.sdk.svg)](https://www.nuget.org/packages/Voucherly.Sdk/)
[![Release](https://github.com/voucherly/voucherly-dotnet-sdk/actions/workflows/release.yml/badge.svg)](https://github.com/voucherly/voucherly-dotnet-sdk/actions/workflows/release.yml)

The official .NET library for the [Voucherly API][api-docs], for .NET 8 and .NET 10. It covers every public operation of the API, and it follows the OpenAPI spec published with the documentation.

Upgrading from 1.x? Read [UPGRADE-2.0.md](https://github.com/voucherly/voucherly-dotnet-sdk/blob/main/UPGRADE-2.0.md).

## Installation

```sh
dotnet add package Voucherly.Sdk
```

## Usage

With dependency injection, register the client and inject `IVoucherlyClient`:

```csharp
builder.Services.AddVoucherly(options => builder.Configuration.GetSection("Voucherly").Bind(options));
```

```json
{
  "Voucherly": {
    "ApiKey": "sk_sand_..."
  }
}
```

Keep the key out of `appsettings.json`: use user secrets in development and an environment variable, `Voucherly__ApiKey`, in production.

Without dependency injection:

```csharp
var voucherly = new VoucherlyClient(new VoucherlyClientOptions { ApiKey = "sk_sand_..." });
```

Then:

```csharp
var payment = await voucherly.Payments.CreateAsync(new CreatePaymentRequest
{
    Mode = PaymentMode.Payment,
    CustomerEmail = "mario.rossi@example.com",
    Lines =
    [
        new PaymentLineRequest
        {
            Quantity = 1,
            UnitAmount = 790,
            Product = new PaymentLineRequestProduct { Name = "Fresh bowl", LineType = LineType.Food },
        },
    ],
    RedirectOkUrl = "https://shop.example.com/ok",
    RedirectKoUrl = "https://shop.example.com/ko",
}, cancellationToken);

return Redirect(payment.CheckoutUrl!);
```

Every tag of the API is a service of the client, and every operation is an async method named after its verb: `voucherly.Payments.RetrieveAsync(id)`, `voucherly.Customers.ListAddressesAsync(customerId, parameters)`, `voucherly.PaymentGateways.ListAsync()`. The [API reference][api-docs] documents every field.

### Requests send only what you set

A request object sends the properties you assign and nothing else, an explicit `null` included. This matters on partial updates, where a missing field is left unchanged and a `null` one is cleared:

```csharp
// Clears the phone number; FirstName and LastName stay as they are.
await voucherly.Customers.UpdateAsync(customerId, new UpdateCustomerRequest { PhoneNumber = null });
```

### Query parameters and pages

Query and header parameters go in a `...Params` object, and required ones are `required` members:

```csharp
var parameters = new ListCustomerParams { Email = "mario.rossi@example.com", Length = 50 };
Page<Customer> page;
do
{
    page = await voucherly.Customers.ListAsync(parameters);
    foreach (var customer in page.Items)
    {
        // ...
    }

    parameters.Start = page.Pagination?.NextStart;
}
while (page.Pagination?.HasMore == true);
```

### Enums

The values of an enum are `const string` fields, such as `PaymentStatus.Paid`, and a property holding one is a plain `string`. A value added to the API later does not break the SDK, and a `switch` still works on the constants.

### Unknown fields

A model keeps the fields this version of the SDK does not know in `ExtensionData`.

### Errors

Every error derives from `Voucherly.Sdk.Exceptions.VoucherlyException`:

- `ApiException` when the API answers with a status outside 2xx. It exposes `StatusCode`, the problem details (`Title`, `Detail`, `ErrorCode`, `Parameter`, `Extensions`), `RawBody` and `Headers`. Each status the spec declares has a subclass: `BadRequestException`, `NotFoundException`, `ConflictException` (with `PaymentStatus`), `UnprocessableEntityException`, `FailedDependencyException`.
- `ConnectionException` when no response arrives: DNS, TLS, refused connection, timeout. A cancellation through the `CancellationToken` stays an `OperationCanceledException`.

```csharp
try
{
    await voucherly.Payments.ConfirmAsync(paymentId, request);
}
catch (ConflictException exception)
{
    // exception.PaymentStatus says why the Payment cannot be confirmed
}
catch (FailedDependencyException exception)
{
    // exception.Extensions["operations"] lists what was already done: sending the same request again settles the rest
}
```

The SDK never retries a request: a payment sent twice can be charged twice.

## HttpClient

The SDK never changes the `BaseAddress` or the `DefaultRequestHeaders` of the `HttpClient` it uses: it sets the key, the headers and the absolute URL on each request, so the same client can be shared.

- `AddVoucherly` returns the `IHttpClientBuilder` of a typed client, so you can add your own handlers, for logging or resilience, and change its timeout, 30 seconds by default.
- `new VoucherlyClient(options, httpClient)` uses your client as it is.
- `new VoucherlyClient(options)` uses a client of its own, with a shared `SocketsHttpHandler`, a 10 seconds connection timeout and a 30 seconds timeout.

## Options

| Option | |
|---|---|
| `ApiKey` | Required. `sk_sand_` for sandbox, `sk_live_` for live, `ik_` for a platform key. |
| `MerchantId`, `Tenant` | Platform keys only. |
| `BaseUrl` | `https://api.voucherly.it` by default. |
| `Os`, `OsVersion`, `OsFramework`, `App`, `AppVersion`, `AppHouse`, `DeviceType` | Telemetry headers, sent only when they have a value. The operating system, the framework and the entry assembly fill them by default. |

## Development

```sh
dotnet test                    # tests and coverage of the spec
dotnet run tools/generate.cs   # rewrites Models, Requests and Enums from spec/openapi.yaml
```

With a sandbox key, `VOUCHERLY_API_KEY=sk_sand_... dotnet test --filter Category=Live` runs read-only calls against the sandbox; add `VOUCHERLY_LIVE_WRITES=1` to run also the calls that create data. The example in `examples/Voucherly.Checkout` reads its key from user secrets: `dotnet user-secrets set Voucherly:ApiKey sk_sand_...`.

For requests, bugs or comments, [open an issue][issues] or [submit a pull request][pulls].

[api-docs]: https://docs.voucherly.it
[issues]: https://github.com/voucherly/voucherly-dotnet-sdk/issues/new
[pulls]: https://github.com/voucherly/voucherly-dotnet-sdk/pulls
