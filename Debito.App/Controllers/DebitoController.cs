using System.Linq;
using System.Threading.Tasks;
using ContaCorrente.ApiDebito.Data;
using ContaCorrente.ApiDebito.Enums;
using ContaCorrente.ApiDebito.Extensions;
using ContaCorrente.ApiDebito.Services;
using ContaCorrente.ApiDebito.ViewModels;
using ContaCorrente.ApiDebito.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContaCorrente.ApiDebito.Controllers
{
    [ApiController]
    [Route("v1/debito")]
    public class DebitoController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAsync(
            [FromServices] TransacaoDataContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            var debito = await context
                .Transacoes
                .AsNoTracking()
                .Where(x => x.TipoTransacao == (int)(ETipoTransacao.Debito))
                .Select(x => new ListTransacoesViewModel
                {
                    Agencia = x.Agencia,
                    Conta = x.Conta,
                    Valor = x.Valor,
                    Descricao = x.Descricao,
                    DataCriacao = x.DataCriacao,
                    TipoTransacao = ((ETipoTransacao)x.TipoTransacao).ToString()
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.DataCriacao)
                .ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = debito.Count,
                page,
                pageSize,
                debito
            }));
        }

        [HttpPost("")]
        public async Task<IActionResult> PostAsync(
            [FromBody] CreateDebitoViewModel model,
            [FromServices] NotificationService notificationService,
            [FromServices] MessageBusService messageBus,
            [FromServices] TransacaoDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

            try
            {
                var tran = model.ToCreateDebito();

                if (!tran.Valor_isValid())
                    return BadRequest(new ResultViewModel<string>("O valor da transação não pode ser menor ou igual a zero."));

                if (!tran.Descricao_Minima())
                    return BadRequest(new ResultViewModel<string>($"A descrição deve conter no minimo {tran.Qtde_Minimia_Caracteres_Descricao()} caracteres."));

                var contaIsValid = await ContaApiClient.GetContaAsync(tran.Agencia, tran.Conta);
                if (!contaIsValid)
                    return BadRequest(new ResultViewModel<string>("Conta não localizada."));

                await context.Transacoes.AddAsync(tran);
                await context.SaveChangesAsync();

                await messageBus.SendAsync(tran, "transacao");

                var result = new ResultViewModel<dynamic>(new
                {
                    Agencia = tran.Agencia,
                    Conta = tran.Conta,
                    Valor = tran.Valor,
                    Descricao = tran.Descricao,
                    DataCriacao = tran.DataCriacao.ToString("dd/MM/yyyy HH:mm:ss"),
                    TipoTransacao = ((ETipoTransacao)tran.TipoTransacao).ToString()
                });

                if (!string.IsNullOrEmpty(model.Webhook))
                    await notificationService.NotifyAsync(model.Webhook, result);


                return Created("", result);
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna servidor"));
            }
        }

    }
}
