using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using SuitsAltering.Contracts;

namespace SuitsAltering.Functions
{
    public class AlteringDoneFunction
    {
        private readonly ILogger<AlteringDoneFunction> _logger;
        private readonly IConfiguration _configuration;

        public AlteringDoneFunction(
            ILogger<AlteringDoneFunction> log,
            IConfiguration configuration)
        {
            _logger = log;
            _configuration = configuration;
        }

        [FunctionName("AlteringDone")]
        [ExponentialBackoffRetry(5, "00:00:04", "00:01:00")]
        public async Task Run([ServiceBusTrigger("order-done", "order-done-subscription", Connection = "ServiceBusConnectionString")] Message message)
        {
            try
            {
                _logger.LogInformation("ServiceBus topic trigger function AlteringDone.");

                var payload = Encoding.UTF8.GetString(message.Body);
                _logger.LogInformation($"ServiceBus AlteringDone processed message: {payload}");

                var model = JsonConvert.DeserializeObject<OrderDone>(payload);

                await SendEmail(model.Email, model.AlteringId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task SendEmail(string email, Guid alteringId)
        {
            var client = new SendGridClient(_configuration.GetValue<string>("SendGridApiKey"));
            var from = new EmailAddress(_configuration.GetValue<string>("EmailFrom"));
            var subject = "Suits altering";
            var to = new EmailAddress(email);
            var plainTextContent = $"Order - {alteringId} is completed.";
            var htmlContent = $"Order - {alteringId} is completed";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (!IsSuccessStatusCode((int)response.StatusCode))
            {
                throw new Exception($"Email hasn't been sent. Status code - {(int)response.StatusCode}");
            }
        }

        private bool IsSuccessStatusCode(int statusCode)
        {
            return (statusCode >= 200) && (statusCode <= 299);
        }
    }



}
