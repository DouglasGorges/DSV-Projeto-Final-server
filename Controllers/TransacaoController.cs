using System.Collections.Generic;
using System;
using System.Linq;
using server.Data;
using server.Models;
using server.Utils;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{

    [ApiController]
    [Route("server/transacao")]
    public class TransacaoController
    {
        private readonly DataContext _context;
        private CategoriaController _categoriaController;
        private CategoriaUtil _categoriaUtil;

        public TransacaoController(DataContext context) => _context = context;

        [HttpPost]
        [Route("create")]
        public Transacao Create(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
            _context.SaveChanges();
            return transacao;
        }

        [HttpGet]
        [Route("list")]
        public List<Transacao> List() => _context.Transacoes.ToList();

        [HttpGet]
        [Route("findById/{id?}")]
        public Transacao GetById(int id) => _context.Transacoes.Find(id);

        [HttpPut]
        [Route("update")]
        public Transacao Update(Transacao transacao)
        {
            Transacao transacaoOriginal = GetById(transacao.Id);

            transacaoOriginal.Descricao = transacao.Descricao;
            transacaoOriginal.ContaCorrente = transacao.ContaCorrente;
            transacaoOriginal.ListaCategorias = transacao.ListaCategorias;
            transacaoOriginal.Valor = transacao.Valor;
            transacaoOriginal.DataVencimento = transacao.DataVencimento;
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