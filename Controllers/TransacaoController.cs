using System.Collections.Generic;
using System.Linq;
using server.Data;
using server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace server.Controllers
{

    [ApiController]
    [Route("server/transacao")]
    public class TransacaoController : ControllerBase
    {
        private readonly DataContext _context;

        public TransacaoController(DataContext context) => _context = context;

        [HttpPost]
        [Route("create")]
        public Transacao Create(Transacao transacao)
        {

            transacao.ContaCorrente = _context.ContasCorrentes.Find(transacao.ContaCorrente.Id);

            List<int> listaIdsCategorias = new List<int>();
            transacao.Categorias.ToList().ForEach(delegate (Categoria categoria)
            {
                listaIdsCategorias.Add(categoria.Id);
            });

            transacao.Categorias.Clear();
            listaIdsCategorias.ForEach(delegate (int idCategoria)
            {
                transacao.Categorias.Add(_context.Categorias.Find(idCategoria));
            });

            _context.Transacoes.Add(transacao);
            _context.SaveChanges();
            return transacao;
        }

        [HttpGet]
        [Route("list")]
        public List<Transacao> List() => _context.Transacoes.Include(t => t.Categorias).Include(t => t.ContaCorrente).ToList();

        [HttpGet]
        [Route("findById/{id?}")]
        public Transacao GetById(int id) => _context.Transacoes.Find(id);

        [HttpPut]
        [Route("update")]
        public Transacao Update(Transacao transacao)
        {
            Transacao transacaoOriginal = GetById(transacao.Id);

            if (transacao.Descricao != null)
                transacaoOriginal.Descricao = transacao.Descricao;
            if (transacao.ContaCorrente != null)
                transacaoOriginal.ContaCorrente = transacao.ContaCorrente;
            if (transacao.Categorias != null && transacao.Categorias.Any())
                transacaoOriginal.Categorias = transacao.Categorias;
            if (transacao.Valor != null)
                transacaoOriginal.Valor = transacao.Valor;
            if (DateTime.Compare(transacao.DataVencimento, DateTime.Parse("01/01/0001 00:00:00")) != 0)
                transacaoOriginal.DataVencimento = transacao.DataVencimento;
            if (transacao.DataPagamento != null)
                transacaoOriginal.DataPagamento = transacao.DataPagamento;

            _context.Transacoes.Update(transacaoOriginal);
            _context.SaveChanges();
            return transacaoOriginal;
        }

        [HttpDelete]
        [Route("delete/{id?}")]
        public Transacao Delete(int id)
        {
            Transacao transacao = GetById(id);
            _context.Transacoes.Remove(transacao);
            _context.SaveChanges();
            return transacao;
        }
    }
}