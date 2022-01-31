using ContaCorrente.ApiExtrato.Data;
using ContaCorrente.ApiExtrato.Enums;
using ContaCorrente.ApiExtrato.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContaCorrente.ApiExtrato.Controllers
{

    [ApiController]
    [Route("v1/extrato")]
    public class TransacoesController : ControllerBase
    {
        [HttpGet("{dias:int}")]
        public async Task<IActionResult> Get(
            [FromServices] TransacoesDataContext context,
            [FromRoute] int dias,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25
        )
        {
            try
            {
                var transacoes = await context
                                .Transacoes
                                .AsNoTracking()
                                .Where(x => x.DataCriacao >= DateTime.Now.AddDays(dias * (-1)))
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
                    total = transacoes.Count,
                    page,
                    pageSize,
                    transacoes
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna servidor"));

            }
        }

        [HttpGet("{agencia}/{conta}")]
        public async Task<IActionResult> GetAgenciaConta(
            [FromServices] TransacoesDataContext context,
            [FromRoute] string agencia,
            [FromRoute] string conta,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25
        )
        {
            try
            {
                var transacoes = await context
                                .Transacoes
                                .AsNoTracking()
                                .Where(x => x.Agencia == agencia
                                        && x.Conta == conta)
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
                    total = transacoes.Count,
                    page,
                    pageSize,
                    transacoes
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna servidor"));

            }
        }

        [HttpGet("{agencia}/{conta}/{dias}")]
        public async Task<IActionResult> GetAgenciaContaDias(
            [FromServices] TransacoesDataContext context,
            [FromRoute] string agencia,
            [FromRoute] string conta,
            [FromRoute] int dias,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25
        )
        {
            try
            {
                var transacoes = await context
                                .Transacoes
                                .AsNoTracking()
                                .Where(x => x.Agencia == agencia
                                        && x.Conta == conta
                                        && x.DataCriacao >= DateTime.Now.AddDays(dias * (-1)))
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
                    total = transacoes.Count,
                    page,
                    pageSize,
                    transacoes
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna servidor"));

            }
        }

    }
}