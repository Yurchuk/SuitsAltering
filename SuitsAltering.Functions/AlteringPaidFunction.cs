using System;
using System.Data.Common;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuitsAltering.Contracts;

namespace SuitsAltering.Functions
{
    public class AlteringPaidFunction
    {
        private readonly ILogger<AlteringPaidFunction> _logger;
        private readonly IConfiguration _configuration;

        public AlteringPaidFunction(
            ILogger<AlteringPaidFunction> log,
            IConfiguration configuration)
        {
            _logger = log;
            _configuration = configuration;
        }

        [FunctionName("AlteringPaid")]
        [ExponentialBackoffRetry(5, "00:00:04", "00:01:00")]
        public async Task Run([ServiceBusTrigger("order-paid", "order-paid-subscription", Connection = "ServiceBusConnectionString")] Message message)
        {
            try
            {
                _logger.LogInformation("ServiceBus topic trigger function AlteringPaid.");

                var payload = Encoding.UTF8.GetString(message.Body);
                _logger.LogInformation($"ServiceBus AlteringPaid processed message: {payload}");

                var model = JsonConvert.DeserializeObject<OrderPaid>(payload);

                var apiUri = _configuration.GetValue<string>("APIUri");

                using var client = new HttpClient();
                client.BaseAddress = new Uri(apiUri);
                var result = await client.PutAsync($"api/altering/setPaidStatus/{model.AlteringId}", new StringContent(""));
                var resultContent = await result.Content.ReadAsStringAsync();
                _logger.LogInformation($"ServiceBus AlteringPaid result message: {resultContent}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }


}
