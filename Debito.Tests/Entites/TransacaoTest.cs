using System;
using ContaCorrente.ApiDebito.Enums;
using ContaCorrente.ApiDebito.Models;
using ContaCorrente.ApiDebito.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Debito.Model.Tests
{
    [TestClass]
    public class TransacaoTest
    {

        [TestMethod]
        [TestCategory("Transacao")]
        public void Transacao_Valor_Invalido_Retorna_False()
        {
            var transacao = new Transacao(
                0,
                "001",
                "001",
                0,
                DateTime.Now,
                "teste",
                Convert.ToInt32(ETipoTransacao.Debito)
            );
            Assert.AreEqual(false, transacao.Valor_isValid());
        }

        [TestMethod]
        [TestCategory("Transacao")]
        public void Transacao_Descricao_Menor_Minima_Retorna_False()
        {
            var transacao = new Transacao(
                0,
                "001",
                "001",
                10,
                DateTime.Now,
                "teste",
                Convert.ToInt32(ETipoTransacao.Debito)
            );
            Assert.AreEqual(false, transacao.Descricao_Minima());
        }

        [TestMethod]
        [TestCategory("Transacao")]
        public void Transacao_Data_Criacao_Diferente_Data_Atual()
        {

            var viewTransacao = new CreateDebitoViewModel
            {
                Agencia = "001",
                Conta = "001",
                Valor = 10,
                Descricao = "teste"
            };

            var transacao = viewTransacao.ToCreateDebito();

            Assert.AreEqual(DateTime.Today, transacao.DataCriacao.Date);

        }

        [TestMethod]
        [TestCategory("Transacao")]
        public void Transacao_Tipo_Transacao_Diferente_Debito()
        {

            var viewTransacao = new CreateDebitoViewModel
            {
                Agencia = "001",
                Conta = "001",
                Valor = 10,
                Descricao = "teste"
            };

            var transacao = viewTransacao.ToCreateDebito();

            Assert.AreEqual(ETipoTransacao.Debito, (ETipoTransacao)transacao.TipoTransacao);

        }
    }
}