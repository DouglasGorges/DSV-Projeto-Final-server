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
    public class ContaCorrenteController
    {
        private readonly DataContext _context;
        private TransacaoController _transacaoController;
        private TransacaoUtil _transacaoUtil;

        public ContaCorrenteController(DataContext context) => _context = context;

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

        [HttpGet]
        [Route("findById/{id?}")]
        public ContaCorrente GetById(int id) => _context.ContasCorrentes.Find(id);

        [HttpPut]
        [Route("update")]
        public ContaCorrente Update(ContaCorrente contaCorrente)
        {
            ContaCorrente contaCorrenteOriginal = GetById(contaCorrente.Id);

            contaCorrenteOriginal.Nome = contaCorrente.Nome;
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
            
            if(_transacaoUtil.ExisteTransacaoComAContaCorrente(contaCorrente)){
                contaCorrente.Ativo = false;
                _context.ContasCorrentes.Update(contaCorrente);
            } else {
                _context.ContasCorrentes.Remove(contaCorrente);
                _context.SaveChanges();
            }

            return contaCorrente;
        }

        [HttpPost]
        [Route("saldo")]
        public double CalcularSaldoTotal() {
            double saldo = 0;

            List<ContaCorrente> listaContasCorrentes = List();

            listaContasCorrentes.ForEach(delegate(ContaCorrente contaCorrente){
                if(contaCorrente.Ativo){
                    saldo += CalcularSaldoContaCorrente(new FiltroPesquisa(contaCorrente));
                }
            });

            return saldo;
        }

        [HttpGet]
        [Route("saldo/{id?}")]
        public double CalcularSaldoContaCorrente(FiltroPesquisa filtro) {

            ContaCorrente contaCorrente = GetById(filtro.ContaCorrente.Id);
            double saldoContaCorrente = contaCorrente.SaldoInicial;

            List<Transacao> listaTransacoes = _transacaoUtil.buscarTransacoesFiltradas(filtro);

            listaTransacoes.ForEach(delegate(Transacao transacao){
                saldoContaCorrente += transacao.Valor;
            });

            return saldoContaCorrente;
        }
    }
}