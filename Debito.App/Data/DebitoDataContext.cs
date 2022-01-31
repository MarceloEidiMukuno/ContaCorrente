using ContaCorrente.ApiDebito.Models;
using Microsoft.EntityFrameworkCore;

namespace ContaCorrente.ApiDebito.Data
{

    public class TransacaoDataContext : DbContext
    {

        public TransacaoDataContext(DbContextOptions options) : base(options) { }

        public DbSet<Transacao> Transacoes { get; set; }

    }
}