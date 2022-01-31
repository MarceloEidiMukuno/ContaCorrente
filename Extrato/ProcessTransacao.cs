using ContaCorrente.ApiExtrato.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ContaCorrente.ApiExtrato.Services;

namespace ContaCorrente.ApiExtrato
{
    public class ProcessTransacao
    {
        private readonly IConfiguration _configuration;
        private readonly IRegisterTransacaoService _registerService;
        private readonly MessageBusService _messageService;
        private const string QUEUE_SALDO = "saldo";

        public ProcessTransacao(IConfiguration configuration)
        {
            _configuration = configuration;

            _registerService = new RegisterTransacaoService(configuration);

            _messageService = new MessageBusService(configuration);

        }

        public void RegisterHandler()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionHandler)
            {
                AutoComplete = false
            };

            _messageService._queueClient.RegisterMessageHandler(ProcessMessageHandler, messageHandlerOptions);
        }

        private async Task ProcessMessageHandler(Message message, CancellationToken cancellationToken)
        {
            var messageString = Encoding.UTF8.GetString(message.Body);
            var transacaoModel = JsonConvert.DeserializeObject<Transacao>(messageString);

            if (transacaoModel.Id != 0)
                transacaoModel.Id = 0;

            await _registerService.RegisterAsync(transacaoModel);

            await _messageService._queueClient.CompleteAsync(message.SystemProperties.LockToken);

            await _messageService.SendAsync(transacaoModel, QUEUE_SALDO);
        }

        private Task ExceptionHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            return Task.CompletedTask;
        }
    }
}