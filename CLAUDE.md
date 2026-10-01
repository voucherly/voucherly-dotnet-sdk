# voucherly-dotnet-sdk

Shared Voucherly conventions live in the `vendor/claude-rules` submodule. After a fresh clone run `git submodule update --init`; to pull a newer version run `git submodule update --remote vendor/claude-rules` and commit the moved pointer.

The submodule is hosted on the private Voucherly Azure DevOps, while this repository is public: a clone without that access builds and tests without it, and the CI does not fetch it.

@vendor/claude-rules/rules/comments.md
@vendor/claude-rules/rules/csharp.md
@vendor/claude-rules/rules/javascript.md
@vendor/claude-rules/rules/testing.md
@vendor/claude-rules/rules/git.md

The official .NET SDK of the Voucherly API, published on NuGet as `Voucherly.Sdk`. It covers every public operation of the OpenAPI spec that the documentation publishes, and nothing more. Its user is Erbert, on net8.0.

## Commands

```sh
dotnet test                                   # xUnit, net8.0 and net10.0; includes the coverage check
dotnet run tools/generate.cs                  # rewrites Models, Requests and Enums from the spec
VOUCHERLY_API_KEY=sk_sand_... dotnet test --filter Category=Live   # read-only calls against the sandbox
```

`VOUCHERLY_LIVE_WRITES=1` adds the live calls that create data in the sandbox: customers, addresses, payments that are then voided, concept stores and store areas that are then deleted, and one store named `SDK test store` that is updated in place. The key only ever lives in the environment, never in a file. `tools/generate.cs` is a file-based app, so it needs the .NET 10 SDK.

## Layout

- `src/Voucherly.Sdk/VoucherlyClient.cs`, `IVoucherlyClient.cs`, `VoucherlyClientOptions.cs`, `ServiceCollectionExtensions.cs`: the client and its registration.
- `src/Voucherly.Sdk/Services/`: one interface and one internal service per tag of the spec, written by hand.
- `src/Voucherly.Sdk/Models/`, `Requests/`, `Enums/`: **generated** by `tools/generate.cs`, except `Page`, `Pagination` and `RequestParams`, which are written by hand. Never edit a generated file: change the generator and run it. The CI fails when the generated files differ from what the generator writes.
- `src/Voucherly.Sdk/VoucherlyObject.cs` and `Http/`: the base of every model, the serializer options, the requestor.
- `src/Voucherly.Sdk/Exceptions/`: the exceptions.
- `spec/openapi.yaml` and `spec/coverage.json`: the copy of the spec and the map from its operations to the methods.
- `examples/Voucherly.Checkout`: a checkout page that creates a Payment; its key comes from user secrets, `Voucherly:ApiKey`.

## Decisions

- **The spec is the contract.** It is read from the `main` branch of the public voucherly-docs repository, `static/download/openapi.yaml`, which is what the documentation site serves. Methods, fields and the text of the doc comments come only from there: the repository is public, and a field the spec does not document stays out on purpose. The spec is written in SberemPay, `docs/dev/webapi/openapi.yaml`, and published to voucherly-docs by `npm run api:publish`: a wrong spec is fixed there, never worked around here.
- **No openapi-generator.** `tools/generate.cs` writes the models, requests, parameters and enums in the style of this repository, and the services are written by hand. `CoverageTests` and `SchemaCoverageTests` prove that nothing of the spec is missing.
- **No `fbognini.Sdk` and no `FastIDs.TypeId`.** The only dependencies are `System.Text.Json` and the `Microsoft.Extensions` abstractions of dependency injection (`Http`, `Options`, `DependencyInjection.Abstractions`). `fbognini.Sdk` was dropped because:
  - its `LoggingHandler` puts the `Voucherly-API-Key` header and the request and response bodies in the logs of whoever uses the SDK;
  - it brings Polly 7 and `Microsoft.Extensions.Http.Polly`, which is deprecated;
  - it exposes its own types, `PaginationResponse<T>` and its exceptions, in the public API, so each of its majors would become a major of the SDK.
- **The `HttpClient` is injected, as in Stripe.NET and Mollie.** `AddVoucherly` registers a typed client and returns its `IHttpClientBuilder`, where SberemPay and Erbert add the `.AddLogging()` of `fbognini.Sdk`. `new VoucherlyClient(options, httpClient)` takes any client; without one, the SDK creates its own on a shared `SocketsHttpHandler` with `PooledConnectionLifetime` set. The SDK never touches `DefaultRequestHeaders` or `BaseAddress`: it puts the key, the headers and the absolute URL on each request, so one `HttpClient` can be shared.
- **An interface per service**, `IPaymentService` and so on, behind `IVoucherlyClient`, so a consumer can substitute them in its tests.
- **Async methods with the `Async` suffix and a `CancellationToken` as the last, optional parameter.**
- **A request sends only the properties assigned to it, an explicit `null` included.** Every property setter calls `MarkAssigned()` through the C# 14 `field` keyword, and a `JsonTypeInfo` modifier serializes only the assigned ones. The API reads which fields are present: on `update-customer`, a missing field is left unchanged and a `null` one is cleared. `LangVersion` is 14 for net8.0 too, which the compiler supports because `field` needs no runtime.
- **Only a class used by requests alone refuses null**, on the properties the spec declares `nullable: false`: a reference type loses its `?` and gets `= null!`, so an unassigned property is still not sent. A response may leave any property out, so the models keep every reference type nullable.
- **A field the spec declares `deprecated: true` is `[Obsolete]`**, with its description as the message.
- **A response accepts what it does not know.** Unknown members go to `ExtensionData`, and enum values are `string` properties with `const string` values, so the server can add fields and values without breaking anybody, and a `switch` still works.
- **Ids are opaque strings**, never parsed.
- **Exceptions**: `VoucherlyException` is the base; `ApiException` carries the status, the problem details already read and the raw body, with one subclass for each error status of the spec; `ConnectionException` is for network errors and timeouts. A cancellation by the caller stays an `OperationCanceledException`.
- **Lists** return a `Page<T>` shaped like the JSON, `Items` and `Pagination`. There is no auto-pagination.
- **Downloads** (`application/pdf`) return a `byte[]`.
- **No automatic retry.** A payment POST sent twice can be charged twice, and the API has no idempotency key.
- **Targets `net8.0` and `net10.0`.** The support of .NET 8 ends in November 2026, but Erbert is on net8.0.
- **Tests use xUnit and Shouldly 4.3.0**, never FluentAssertions, which has a commercial licence from v8. The fake transport is an `HttpMessageHandler`, and the coverage check is a test that reads `spec/openapi.yaml` with YamlDotNet, which only the test project references.

## What the server does that the spec does not say

- It still accepts `isFood`, on the line and on the product, as an obsolete alias of `lineType`. The SDK sends only `lineType`.
- It answers 415 to a POST without a JSON body, even on `void-payment`, which declares none: a POST or PUT without a request sends `{}`.
- Some date-times come without an offset, such as `2026-09-08T01:24:43.0446376` or `0001-01-01T00:00:00.0000000`. They are UTC, so `UtcDateTimeOffsetConverter` reads them as UTC: the default converter would take the local offset, shifting the instant, and fail on the minimum value east of Greenwich.
- It reads five telemetry headers, `x-voucherly-os`, `x-voucherly-osversion`, `x-voucherly-app`, `x-voucherly-appversion`, `x-voucherly-devicetype`. The SDK also keeps `x-voucherly-osframework` and `x-voucherly-apphouse`, as 1.x did. A header without a value is not sent.
- One host, `https://api.voucherly.it`, whose paths already hold `/v1`. Sandbox and live differ by the prefix of the key, not by host.

The User-Agent is `VoucherlyApiDotnetSdk/<version>`, with the version of the package.

## Naming rule

- **Service**: one per tag, a property of the client in the plural: `Companies`, `Customers`, `PaymentMethods`, `Payments`, `PaymentGateways`, `Receipts`, `Terminals`, `Stores`, `ConceptStores`, `StoreAreas`, `Reports`.
- **Method**: the verb of the `operationId`, followed by the words that come after the name of the tag's resource, in the plural for a `list`, with the `Async` suffix: `create-payment` is `Payments.CreateAsync`, `list-customer-address` is `Customers.ListAddressesAsync`, `retrieve-customer-prepaid-balance` is `Customers.RetrievePrepaidBalanceAsync`, `download-payment-refund-receipt` is `Payments.DownloadRefundReceiptAsync`, `list-customer-payment-method` is `PaymentMethods.ListAsync`, `volumes-report` is `Reports.VolumesAsync`. The rule has no exception today; if one becomes necessary, write it here.
- **Arguments**: the path parameters with the names of the spec, then the request body, then the `...Params` object with the query and header parameters, then the `CancellationToken`. A `...Params` object is always an object, even for one parameter, so that a new parameter is a minor release and does not change the binary signature.
- **Classes**: a schema drops its group prefix (`Payments.`, `Customers.`, `Stores.`, `PaymentGateways.`, `Receipts.`, `Terminals.`, `Reports.`) and joins the rest: `Payments.PaymentLine.Unit` is `PaymentLineUnit`. An inline object is its parent followed by the property: `PaymentLineRequestProduct`. The renames are `CompanyAddressForExternalApi` to `CompanyAddress`, `GetPaymentGatewaysResponse` to `PaymentGatewayList`, `PaginationResponse` to `Pagination`, and the volumes response to `VolumesReport`. The inline enums are named in `InlineEnums` and `IncludeEnums` of the generator.
- **Parameters**: `<OperationId in PascalCase>Params`, with the required parameters as `required` members; a header drops its `Voucherly-` prefix, so `Voucherly-Wait-Time` is `WaitTime`.
- **Enum values**: the value itself, with the first letter upper case: `NonFood`, `OTP_CF`, `Live`.
- **Classes used only by request bodies** go in `Voucherly.Sdk.Requests`, every other class in `Voucherly.Sdk.Models`.

## The spec and its updates

`spec/openapi.yaml` is committed: its `git diff` is the list of changes of the next update. `spec/coverage.json` maps every public `operationId` to its method, `Service.MethodAsync`, or excludes it with a reason, in the same format as the PHP SDK. The procedure of an update is the `sync-api` skill in `.claude/skills/sync-api/`.

`.github/workflows/sync-api.yml` does the mechanical part of it, without Claude and without any API key:

- it starts on a `repository_dispatch` of type `openapi-updated`, which voucherly-docs sends at every push on `main` that touches `static/download/openapi.yaml`, with `client_payload: { "sha": "<docs commit>" }`; by hand with `workflow_dispatch`, whose `sha` input defaults to `main`; and every Monday, because a dispatch whose token has expired never arrives and nobody notices;
- it downloads the spec at that commit and stops when it is the one already on `main`, or on the open `sync-api/*` pull request;
- otherwise it runs `dotnet run tools/generate.cs`, the tests and the coverage check, commits the spec and the generated code on `sync-api/<date>`, and opens a pull request towards `main`, or updates the one already open, with the result of each check and the diff of the spec.

What the generator cannot do stays by hand, on that branch, with the skill: the methods, `spec/coverage.json` and the tests of new or removed operations, the kind of release, the version and the changelog. The workflow pushes the branch and opens the pull request with `SDK_TOKEN`, not with `GITHUB_TOKEN`: the organization does not let `GITHUB_TOKEN` open pull requests, and a pull request opened with it would start no CI. Merge, tag and release stay by hand, so that a major never ships without Francesco reading it.

`SDK_TOKEN` is an organization secret shared with voucherly-docs and the two SDK repositories: a fine-grained token with Contents and Pull requests write permission on the two SDK repositories. The dispatch is sent by `.github/workflows/notify-sdks.yml` of voucherly-docs, with the same secret. The contract between the repositories is only the `event_type` and the `client_payload` above.

An automatic update reads the spec from GitHub at the commit that triggered it, `https://raw.githubusercontent.com/voucherly/voucherly-docs/<sha>/static/download/openapi.yaml`, and never from the documentation site. Netlify finishes deploying after the push, so the site can still serve the previous spec, and the address with `main` can stay cached for a few minutes, while the one with the sha always returns the same file.

## Tests

- `FakeHttpMessageHandler` records the requests and returns queued responses; `ServiceTestBase` checks each operation against the spec, with its method, path, query, headers and body.
- The spec has no example of a 2xx response, so each service test builds the response from the schema with every property, and requires the model to read all of it and nothing else.
- The request examples of the spec, named examples included, are sent through their classes and must come out unchanged.
- `SchemaCoverageTests` compares the request bodies and their nullability, the enums and the parameters with the spec; `CoverageTests` checks `spec/coverage.json` against the spec and `IVoucherlyClient`.
- `Live/` runs against the sandbox, only with a sandbox key in `VOUCHERLY_API_KEY`.

## Conventions

- Comments and commit messages follow the imported `comments.md` and `git.md`.
- No commit, stage, tag or push unless Francesco asks for it.
- A field removed or renamed, or a method whose signature changes, is a major release.
- A release is a pushed tag `vX.Y.Z`, as in the PHP SDK. Bump `<Version>` in the csproj and add the `## X.Y.Z - date` section to `CHANGELOG.md` first: a test checks that they agree. `.github/workflows/release.yml` checks the tag against the version and the changelog, runs the CI, packs with the version of the tag and pushes it to NuGet through trusted publishing, then creates the GitHub release with the notes of that section. No API key is stored: the trusted publishing policy on nuget.org, owned by the `voucherly` organization, names the repository `voucherly/voucherly-dotnet-sdk`, the workflow file `release.yml` and the environment `nuget`, so renaming the workflow or the environment breaks the publish. The policy cannot check the ref, so the `nuget` environment on GitHub admits only the tags `v*.*.*`: a `release.yml` edited on a branch cannot publish. The `NUGET_USER` secret holds the nuget.org profile name of a member of the organization. The GitHub release comes last, so that it always means a package on NuGet. Never create a GitHub release by hand: it publishes nothing, and the workflow then fails to create its own.
