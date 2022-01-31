using ContaCorrente.ApiExtrato.Models;
using System.Threading.Tasks;

namespace ContaCorrente.ApiExtrato.Services
{
    public interface IRegisterTransacaoService
    {
        Task RegisterAsync(Transacao transacao);
    }
}