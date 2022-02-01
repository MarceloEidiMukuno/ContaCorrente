
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace ContaCorrente.ApiDebito.Services
{

    public class MessageBusService
    {

        private readonly IConfiguration _configuration;

        public MessageBusService(IConfiguration configuration) => _configuration = configuration;

        public async Task SendAsync<T>(T data, string queue)
        {
            var connectionString = _configuration.GetSection("ServiceConnectionString").Value;
            await using var client = new ServiceBusClient(connectionString);

            var sender = client.CreateSender(queue);
            var json = JsonSerializer.Serialize(data);
            var message = new ServiceBusMessage(json);

            await sender.SendMessageAsync(message);
        }
    }
}