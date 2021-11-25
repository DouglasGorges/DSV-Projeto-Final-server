using System.Collections.Generic;
using System.Linq;
using server.Data;
using server.Models;
using server.Utils;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{

    [ApiController]
    [Route("server/contaCorrente")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly DataContext _context;
        private TransacaoUtil _transacaoUtil;

        public ContaCorrenteController(DataContext context)
        {
            _context = context;
            _transacaoUtil = new TransacaoUtil(_context);
        }

        [HttpPost]
        [Route("create")]
        public ContaCorrente Create(ContaCorrente contaCorrente)
        {
            _context.ContasCorrentes.Add(contaCorrente);
            _context.SaveChanges();
            return contaCorrente;
        }

        [HttpGet]
        [Route("list")]
        public List<ContaCorrente> List() => _context.ContasCorrentes.ToList();

        public List<ContaCorrente> ListarAtivas() => _context.ContasCorrentes.Where(cc => cc.Ativo).ToList();

        [HttpGet]
        [Route("findById/{id?}")]
        public ContaCorrente GetById(int id) => _context.ContasCorrentes.Find(id);

        [HttpPut]
        [Route("update")]
        public ContaCorrente Update(ContaCorrente contaCorrente)
        {
            ContaCorrente contaCorrenteOriginal = GetById(contaCorrente.Id);

            if (contaCorrente.Nome != null)
                contaCorrenteOriginal.Nome = contaCorrente.Nome;
            if (contaCorrente.Ativo != contaCorrenteOriginal.Ativo)
                contaCorrenteOriginal.Ativo = contaCorrente.Ativo;
            if (contaCorrente.SaldoInicial != null)
                contaCorrenteOriginal.SaldoInicial = contaCorrente.SaldoInicial;

            _context.ContasCorrentes.Update(contaCorrenteOriginal);
            _context.SaveChanges();
            return contaCorrenteOriginal;
        }

        [HttpDelete]
        [Route("delete/{id?}")]
        public ContaCorrente Delete(int id)
        {
            ContaCorrente contaCorrente = GetById(id);

            if (_transacaoUtil.ExisteTransacaoComAContaCorrente(contaCorrente))
            {
                contaCorrente.Ativo = false;
                _context.ContasCorrentes.Update(contaCorrente);
            }
            else
            {
                _context.ContasCorrentes.Remove(contaCorrente);
                _context.SaveChanges();
            }

            return contaCorrente;
        }

        [HttpPost]
        [Route("saldoTotal")]
        public double CalcularSaldoTotal()
        {
            double saldo = 0;

            List<ContaCorrente> listaContasCorrentes = ListarAtivas();
            listaContasCorrentes.ForEach(delegate (ContaCorrente contaCorrente)
            {
                double a = CalcularSaldoContaCorrente(new FiltroPesquisa(contaCorrente, StatusTransacao.Pago));
                saldo += a;
            });

            return saldo;
        }

        [HttpPost]
        [Route("saldo")]
        public double CalcularSaldoContaCorrente(FiltroPesquisa filtro)
        {
            ContaCorrente contaCorrente = GetById(filtro.ContaCorrente.Id);
            double saldoContaCorrente = contaCorrente.SaldoInicial.GetValueOrDefault(0);

            List<Transacao> listaTransacoes = _transacaoUtil.buscarTransacoesFiltradas(filtro);
            listaTransacoes.ForEach(delegate (Transacao transacao)
            {
                if (transacao.Valor != null)
                    saldoContaCorrente += transacao.Valor.GetValueOrDefault(0);
            });

            return saldoContaCorrente;
        }
    }
}