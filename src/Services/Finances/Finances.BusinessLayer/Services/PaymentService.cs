using Finances.BusinessLayer.Contracts;
using Finances.BusinessLayer.Exceptions.ServerExceptions;
using Finances.BusinessLayer.Models.YooKassa;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Finances.BusinessLayer.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly string _url = "https://api.yookassa.ru/v3/payment/";
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client = new HttpClient();
        public PaymentService(IConfiguration configuration) =>
            _configuration = configuration;

        public async Task<bool> ConfirmPayment(Guid paymentId)
        {
            HttpResponseMessage response = await GetResponse(_url);
            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

        public async Task<bool> ConfirmPaymentData(Guid userId, Guid financeId)
        {
            HttpResponseMessage response = await GetResponse(_url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                YooKassaSuccessResponse responseGetModel = JsonConvert.DeserializeObject<YooKassaSuccessResponse>(json);

                if (responseGetModel.Metadata is null) return false;

                if (responseGetModel.Metadata.UserId == userId && responseGetModel.Metadata.FinanceId == financeId)
                    return true;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new InternalServerException("Something went wrong on yookassa server");

            return false;
        }

        public async Task<HttpResponseMessage> GetResponse(string url)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _url);

            SetCredentials(request);

            return await _client.SendAsync(request);
        }

        private void SetCredentials(HttpRequestMessage request)
        {
            string credentials = Convert
                .ToBase64String(Encoding.ASCII
                .GetBytes(_configuration["YooKassa:Username"] + ":" + _configuration["YooKassa:Password"]));

            request.Headers.Add("Authorization", "Basic" + credentials);
        }
    }
}
