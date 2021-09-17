using System.Collections.Generic;
using System.Linq;
using server.Data;
using server.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{

    [ApiController]
    [Route("server/contaCorrente")]
    public class ContaCorrenteController
    {
        private readonly DataContext _context;

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
            contaCorrenteOriginal.Saldo = contaCorrente.Saldo;

            _context.ContasCorrentes.Update(contaCorrenteOriginal);
            _context.SaveChanges();
            return contaCorrenteOriginal;
        }

        [HttpDelete]
        [Route("delete/{id?}")]
        public ContaCorrente Delete(int id)
        {
            ContaCorrente contaCorrente = GetById(id);
            _context.ContasCorrentes.Remove(contaCorrente);
            _context.SaveChanges();
            return contaCorrente;
        }
    }
}