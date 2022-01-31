using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace ContaCorrente.ApiExtrato.Services
{

    public class MessageBusService
    {

        private readonly IConfiguration _configuration;
        public readonly QueueClient _queueClient;

        private string _connectionString;

        private const string QUEUE_TRANSACAO = "transacao";

        public MessageBusService(IConfiguration configuration)
        {

            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("ServiceConnectionString");

            _queueClient = new QueueClient(_connectionString, QUEUE_TRANSACAO);
        }
        public async Task SendAsync<T>(T data, string queue)
        {
            await using var client = new ServiceBusClient(_connectionString);

            var message = new ServiceBusMessage(JsonSerializer.Serialize(data));

            var sender = client.CreateSender(queue);
            await sender.SendMessageAsync(message);
        }
    }
}