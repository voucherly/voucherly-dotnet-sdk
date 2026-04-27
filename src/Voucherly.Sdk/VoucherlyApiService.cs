using FastIDs.TypeId.Serialization.SystemTextJson;
using fbognini.Sdk;
using fbognini.Sdk.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Voucherly.Sdk.Endpoints;
using Voucherly.Sdk.Models.Customers;
using Voucherly.Sdk.Models.PaymentGateways;
using Voucherly.Sdk.Models.PaymentMethods;
using Voucherly.Sdk.Models.Payments;
using Voucherly.Sdk.Requests;

namespace Voucherly.Sdk
{
    public interface IVoucherlyApiService
    {
        static JsonSerializerOptions JsonSerializerOptions => 
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() },
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }
            .ConfigureForTypeId();

        #region Payments
        Task<Payment> CreatePayment(CreatePaymentRequest request);
        Task<Payment> ConfirmPayment(string id);
        Task<Payment> GetPayment(string id, GetPaymentRequest request);
        Task<Payment> RefundPayment(string id);

        #endregion

        #region Customer

        Task<PaginationResponse<PaymentMethod>> GetCustomerPaymentMethods(string id);
        Task DeletePaymentMethod(string customerId, string id);

        #endregion

        #region Payment Gateways
        Task<PaymentGatewaysResponse> GetPaymentGateways(GetPaymentGatewaysRequest request);
        #endregion
    }

    internal class VoucherlyApiService : BaseApiService, IVoucherlyApiService
    {
        private readonly VoucherlyApiSettings _settings;

        public VoucherlyApiService(HttpClient client, IOptions<VoucherlyApiSettings> options)
            : base(client, options: IVoucherlyApiService.JsonSerializerOptions)
        {
            _settings = options.Value;

            this.client.BaseAddress = new Uri("https://api.voucherly.it/");
            this.client.DefaultRequestHeaders.Add("Voucherly-API-Key", _settings.ApiKey);
            this.client.DefaultRequestHeaders.Add("x-voucherly-os", _settings.Os);
            this.client.DefaultRequestHeaders.Add("x-voucherly-osversion", _settings.OsVersion);
            this.client.DefaultRequestHeaders.Add("x-voucherly-osframework", _settings.OsFramework);
            this.client.DefaultRequestHeaders.Add("x-voucherly-app", _settings.App);
            this.client.DefaultRequestHeaders.Add("x-voucherly-appversion", _settings.AppVersion);
            this.client.DefaultRequestHeaders.Add("x-voucherly-apphouse", _settings.AppHouse);
            this.client.DefaultRequestHeaders.Add("x-voucherly-devicetype", _settings.DeviceType);
            this.client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("VoucherlyApiDotnetSdk", Assembly.GetExecutingAssembly().GetName().Version?.ToString()));
        }

        public async Task<Payment> GetPayment(string id, GetPaymentRequest request)
        {
            var options = new RequestOptions();
            if (request.VoucherlyWaitTime.HasValue)
            {
                options.Headers.Add("Voucherly-Wait-Time", request.VoucherlyWaitTime.Value.ToString());
            }

            return await GetApiAsync<Payment>(PaymentEndpoints.GetPayment(id, request?.Includes ?? Enumerable.Empty<PaymentIncludes>()));
        }

        public async Task<Payment> CreatePayment(CreatePaymentRequest request)
        {
            return await PostApiAsync<Payment, CreatePaymentRequest>(PaymentEndpoints.CreatePayment(), request);
        }

        public async Task<Payment> ConfirmPayment(string id)
        {
            return await PostApiAsync<Payment>(PaymentEndpoints.ConfirmPayment(id));
        }

        public async Task<Payment> RefundPayment(string id)
        {
            return await PostApiAsync<Payment>(PaymentEndpoints.RefundPayment(id));
        }

        public async Task<PaginationResponse<PaymentMethod>> GetCustomerPaymentMethods(string id)
        {
            return await GetApiAsync<PaginationResponse<PaymentMethod>>(CustomerEndpoints.GetPaymentMethods(id));
        }

        public async Task DeletePaymentMethod(string customerId, string id)
        {
            await DeleteApiAsync(CustomerEndpoints.DeletePaymentMethod(customerId, id));
        }

        public async Task<PaymentGatewaysResponse> GetPaymentGateways(GetPaymentGatewaysRequest request)
        {
            return await GetApiAsync<PaymentGatewaysResponse>(PaymentGatewayEndpoints.GetPaymentGateways(request.All, request.Includes ?? Enumerable.Empty<PaymentGatewayIncludes>()));
        }
    }
}