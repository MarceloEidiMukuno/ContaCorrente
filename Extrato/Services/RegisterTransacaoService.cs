using ContaCorrente.ApiExtrato.Data;
using ContaCorrente.ApiExtrato.Models;

namespace ContaCorrente.ApiExtrato.Services
{
    public class RegisterTransacaoService : IRegisterTransacaoService
    {
        private readonly IConfiguration _configuration;
        private readonly TransacoesDataContext _context;

        public RegisterTransacaoService(IConfiguration configuration)
        {

            _configuration = configuration;

            _context = new TransacoesDataContext(configuration);

        }

        public async Task RegisterAsync(Transacao transacao)
        {

            await _context.Transacoes.AddAsync(transacao);
            await _context.SaveChangesAsync();

        }
    }
}