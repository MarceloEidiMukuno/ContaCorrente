using ContaCorrente.ApiExtrato.Models;
using Microsoft.EntityFrameworkCore;

namespace ContaCorrente.ApiExtrato.Data
{
    public class TransacoesDataContext : DbContext
    {

        private readonly IConfiguration _configuration;

        public TransacoesDataContext(IConfiguration configuration) => _configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("DataConnectionStrings"));
        }

        public DbSet<Transacao> Transacoes { get; set; }
    }
}
