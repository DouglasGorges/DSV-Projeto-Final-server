using System.Collections.Generic;
using System.Linq;
using server.Data;
using server.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [ApiController]
    [Route("server/produto")]

    public class ProdutoController : ControllerBase
    {
        private readonly DataContext _context;

        //Construtor
        public ProdutoController(DataContext context) => _context = context;

        [HttpPost]
        [Route("create")]
        public Produto Create(Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        [HttpGet]
        [Route("list")]
        public List<Produto> List() => _context.Produtos.ToList();

        [HttpGet]
        [Route("findById/{id?}")]
        public Produto GetById(int id) => _context.Produtos.Find(id);

        [HttpPost]
        [Route("edit")]
        public Produto Edit(Produto produto)
        {
            Produto produtoOriginal = GetById(produto.Id);

            produtoOriginal.Descricao = produto.Descricao;
            //E outros atributos

            _context.Produtos.Update(produtoOriginal);
            _context.SaveChanges();
            return produtoOriginal;
        }

        [HttpPost]
        [Route("remove/{id?}")]
        public Produto Remove(int id)
        {
            Produto produto = GetById(id);
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return produto;
        }
    }
}