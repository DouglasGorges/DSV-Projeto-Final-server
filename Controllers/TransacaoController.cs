using System.Collections.Generic;
using System.Linq;
using server.Data;
using server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            /*
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
            */

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

            transacaoOriginal.Descricao = transacao.Descricao;
            transacaoOriginal.ContaCorrente = transacao.ContaCorrente;
            transacaoOriginal.Categorias = transacao.Categorias;
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