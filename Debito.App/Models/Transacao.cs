using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContaCorrente.ApiDebito.Enums;

namespace ContaCorrente.ApiDebito.Models
{
    [Table("Transacoes")]
    public class Transacao
    {
        public const int VALORMINIMODESCRICAO = 10;

        public Transacao(int TransacaoId, string Agencia, string Conta, decimal Valor, DateTime DataCriacao, string Descricao, int TipoTransacao)
        {
            this.TransacaoId = TransacaoId;
            this.Agencia = Agencia;
            this.Conta = Conta;
            this.Valor = Valor;
            this.DataCriacao = DataCriacao;
            this.Descricao = Descricao;
            this.TipoTransacao = TipoTransacao;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransacaoId { get; private set; }

        [Required]
        [MaxLength(80)]
        [Column("Agencia", TypeName = "NVARCHAR")]
        public string Agencia { get; private set; }

        [Required]
        [MaxLength(80)]
        [Column("Conta", TypeName = "NVARCHAR")]
        public string Conta { get; private set; }

        [Required]
        [Column("Valor", TypeName = "DECIMAL(18,2)")]
        public decimal Valor { get; private set; }

        [Required]
        [Column("DataCriacao", TypeName = "DATETIME")]
        public DateTime DataCriacao { get; private set; }

        [Required]
        [MaxLength(200)]
        [Column("Descricao", TypeName = "NVARCHAR")]
        public string Descricao { get; private set; }

        [Required]
        [Column("TipoTransacao", TypeName = "INT")]
        public int TipoTransacao { get; private set; }

        public bool Valor_isValid()
        {
            return Valor > 0;
        }

        public bool Descricao_Minima()
        {
            return Descricao.Length >= VALORMINIMODESCRICAO;
        }

        public int Qtde_Minimia_Caracteres_Descricao()
        {
            return VALORMINIMODESCRICAO;
        }

    }
}