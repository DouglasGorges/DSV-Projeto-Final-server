using System;
using System.Collections.Generic;

namespace server.Models
{
    public class Categoria
    {
        public Categoria() { CriadoEm = DateTime.Now; Ativo = true; }

        public int Id { get; set;}
        public string Nome { get; set;}
        public string Cor { get; set;}
        public bool Ativo { get; set;}
        public virtual ICollection<Transacao> Transacoes { get; set;}
        public List<CategoriaTransacao> CategoriasTransacoes { get; set;}
        public DateTime CriadoEm { get; set;}

        public override string ToString() =>
            $"Nome: {Nome} | Cor: {Cor} | Criado em: {CriadoEm}";
   
    }
}