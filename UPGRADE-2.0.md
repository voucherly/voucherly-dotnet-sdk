# Upgrading from 1.x to 2.0

2.0 is a rewrite: `IVoucherlyApiService` is replaced by `IVoucherlyClient`, which has one service per tag of the API. This page translates every 1.x call.

## Registration

1.x read the options from the `VoucherlyApiSettings` section and added the logging handler of `fbognini.Sdk`:

```csharp
services.AddVoucherlyApiService(configuration);
```

2.0 takes the options through a delegate, and returns the `IHttpClientBuilder` so you add the handlers you want. The option names have not changed, so the same section still binds:

```csharp
services.AddVoucherly(options => configuration.GetSection("VoucherlyApiSettings").Bind(options))
    .AddLogging(); // fbognini.Sdk, only if you want its logging
```

The logging handler of `fbognini.Sdk` writes the `Voucherly-API-Key` header and the request and response bodies to your logs: add it only where that is acceptable.

Inject `IVoucherlyClient` where you injected `IVoucherlyApiService`.

1.x accepted an empty key and let the API answer 401 at the first call. 2.0 throws an `ArgumentException` when the client is created without a key: `new VoucherlyClient(options)` throws at once, and with `AddVoucherly` every resolution of `IVoucherlyClient` throws, so every class that injects it fails to be created. Where the key may not be configured yet, resolve `IVoucherlyClient` from the `IServiceProvider` when you first need it, not in a constructor, and treat an empty key as an invalid one.

## Calls

| 1.x | 2.0 |
|---|---|
| `CreatePayment(request)` | `Payments.CreateAsync(request, cancellationToken)` |
| `GetPayment(id, new GetPaymentRequest { Includes = …, VoucherlyWaitTime = 30 })` | `Payments.RetrieveAsync(id, new RetrievePaymentParams { Include = [PaymentInclude.Lines], WaitTime = 30 })` |
| `ConfirmPayment(id)` | `Payments.ConfirmAsync(id)` |
| `RefundPayment(id)` | `Payments.RefundAsync(id)` |
| `GetCustomerPaymentMethods(customerId)` | `PaymentMethods.ListAsync(customerId)` |
| `DeletePaymentMethod(customerId, id)` | `PaymentMethods.DeleteAsync(customerId, id)` |
| `GetPaymentGateways(new GetPaymentGatewaysRequest { All = true, Includes = … })` | `PaymentGateways.ListAsync(new ListPaymentGatewayParams { All = true, Include = [PaymentGatewayInclude.Parameters] })` |

`GetPayment` never sent `VoucherlyWaitTime` in 1.x, so a call that passed it did not wait: in 2.0, `WaitTime` makes the call hang until the status changes, up to 60 seconds.

`ConfirmAsync` and `RefundAsync` accept an optional request: without it they confirm or refund the whole Payment, as 1.x did.

The API pages the saved PaymentMethods like every other list: `PaymentMethods.ListAsync(customerId)` returns the 10 most recent, and `new ListCustomerPaymentMethodParams { Length = 100 }` asks for up to 100. `Pagination.NextStart` goes in `Start` to read the next page.

## Payment requests

| 1.x | 2.0 |
|---|---|
| `Voucherly.Sdk.Requests.CreatePaymentRequest` | same name and namespace |
| `CreatePaymentRequest.PaymentLine` | `PaymentLineRequest` |
| `CreatePaymentRequest.PaymentLineProduct` | `PaymentLineRequestProduct` |
| `PaymentLineProduct.IsFood = true` | `PaymentLineRequestProduct.LineType = LineType.Food` |
| `CreatePaymentRequest.PaymentDiscount` | `Voucherly.Sdk.Models.PaymentDiscount` |
| `PaymentLine.ProductId` as `TypeId` | `string` |
| `SelectedPaymentGateway` | `SelectedPaymentGatewayId` |
| `IsPartialPayment`, `PaymentGateways` | removed: the API does not have them; see `CompletionMode` in the API reference |
| amounts as `long` | `int`, as the spec declares them |

A field you do not assign is no longer sent: 1.x sent the customer fields and the redirect URLs as empty strings, and `Mode` even when you did not assign it. The API has no default for `mode` and answers 400 to a request without it, so assign it: `Mode = PaymentMode.Payment`.

Up to 1.4.x the product fields were flat on `CreatePaymentRequest.PaymentLine`. They move to `PaymentLineRequestProduct`, and none of them is gone:

| 1.4.x `CreatePaymentRequest.PaymentLine` | 2.0 |
|---|---|
| `Quantity`, `UnitAmount`, `UnitDiscountAmount`, `DiscountAmount` | same name on `PaymentLineRequest` |
| `ProductId` as `int?` | `PaymentLineRequest.ProductId` as `string` |
| `ProductName` | `Product.Name` |
| `ProductDescription` | `Product.Variant` |
| `ProductImage` | `Product.Image` |
| `TaxRate` | `Product.TaxRate` |
| `IsFood = true` | `Product.LineType = LineType.Food` |
| `PriceId` | removed: the API does not have it |

```csharp
var line = new PaymentLineRequest
{
    Quantity = quantity,
    UnitAmount = unitAmountInCents,
    Product = new PaymentLineRequestProduct
    {
        Name = name,
        Variant = description,
        Image = imageUrl,
        TaxRate = 22.0,
        LineType = isFood ? LineType.Food : LineType.NonFood,
    },
};
```

## Models

The models moved to `Voucherly.Sdk.Models`, without the sub-namespaces: `Voucherly.Sdk.Models.Payments.Payment` is `Voucherly.Sdk.Models.Payment`.

The enums are `const string` values in `Voucherly.Sdk.Enums`, and the properties that hold them are `string`:

| 1.x | 2.0 |
|---|---|
| `payment.Status == PaymentStaus.Paid` | `payment.Status == PaymentStatus.Paid` |
| `PaymentMode.Payment` (enum) | `PaymentMode.Payment` (string) |
| `CheckoutAction.REDIRECT` (enum) | `CheckoutAction.REDIRECT` (string) |
| `DiscountType.DOLLAROFF` (enum) | `DiscountType.DOLLAROFF` (string) |

`Payment.PaymentGateways` and `Payment.RedirectUrl` are gone, as the API no longer returns them. Lists are `Page<T>` with `Items` and `Pagination`, as `PaginationResponse<T>` of `fbognini.Sdk` was.

On a `PaymentLine` read from the API, the `ProductDescription` of 1.4.x is `ProductVariant`, as in 1.5. `TotalAmount`, `TotalDiscountAmount` and `IsFood` are still there, but `[Obsolete]` because the API deprecates them: compute the totals from `UnitAmount`, `UnitDiscountAmount` and `Quantity`, and read `LineType`.

`Metadata` is a `Dictionary<string, string>`, as in 1.x, on `CreatePaymentRequest` and `Payment`, and now also on `Customer` and its requests. A JSON object without a fixed shape, such as `TransactionNextAction.Params`, is a `Dictionary<string, JsonElement>`.

## Errors

The `ApiException` of `fbognini.Sdk` becomes `Voucherly.Sdk.Exceptions.ApiException`, with `StatusCode`, `Title`, `Detail`, `ErrorCode`, `Parameter`, `Extensions`, `RawBody` and `Headers`. The body that 1.x kept in `Content` is `RawBody`. Network errors and timeouts are `ConnectionException`; both derive from `VoucherlyException`.
