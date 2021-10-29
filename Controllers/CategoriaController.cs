using System.Collections.Generic;
using System.Linq;
using server.Data;
using server.Models;
using server.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

namespace server.Controllers
{

    [ApiController]
    [Route("server/categoria")]
    public class CategoriaController : ControllerBase
    {
        private readonly DataContext _context;
        TransacaoUtil _transacaoUtil;

        public CategoriaController(DataContext context){
            _context = context;
            _transacaoUtil = new TransacaoUtil(_context);
        }

        [HttpPost]
        [Route("create")]
        public Categoria Create(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return categoria;
        }

        [HttpGet]
        [Route("list")]
        public List<Categoria> List() {
            List<Categoria> listaCategorias = _context.Categorias.ToList();
            List<Categoria> listaCategoriasAtivas = new List<Categoria>();

            listaCategorias.ForEach(delegate(Categoria categoria){
                if(categoria.Ativo){
                    listaCategoriasAtivas.Add(categoria);
                }
            });
            return listaCategoriasAtivas;
            }

        private Categoria GetById(int id) => _context.Categorias.Find(id);

        [HttpPut]
        [Route("update")]
        public Categoria Update(Categoria categoria)
        {
            Categoria categoriaOriginal = GetById(categoria.Id);
            
            if(categoria.Nome != null)
                categoriaOriginal.Nome = categoria.Nome;
            if(categoria.Cor != null)
                categoriaOriginal.Cor = categoria.Cor;
            categoriaOriginal.Ativo = categoria.Ativo;

            _context.Categorias.Update(categoriaOriginal);
            _context.SaveChanges();
            return categoriaOriginal;
        }

        [HttpDelete]
        [Route("delete/{id?}")]
        public Categoria Delete(int id)
        {
            Categoria categoria = GetById(id);
            
            if(_transacaoUtil.ExisteTransacaoComACategoria(categoria)){
                categoria.Ativo = false;
                _context.Categorias.Update(categoria);
            } else {
                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
            }

            return categoria;
        }

    }
}