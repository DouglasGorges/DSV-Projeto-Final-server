using System;
using System.Collections.Generic;

namespace server.Models
{
    public class Transacao
    {
        public Transacao() => CriadoEm = DateTime.Now;

        public int Id { get; set;}
        public string Descricao { get; set;}
        public ContaCorrente ContaCorrente { get; set;}
        public virtual ICollection<Categoria> Categorias { get; set;}
        public List<CategoriaTransacao> CategoriasTransacoes { get; set;}
        public double? Valor { get; set;}
        public DateTime DataVencimento { get; set;}
        public DateTime? DataPagamento { get; set;}
        public DateTime CriadoEm { get; set;}

        public override string ToString() =>
            $"Nome: {Descricao} | Valor: {Valor:C2} | Vencimento: {DataVencimento} | Pagamento: {DataPagamento}";
   
    }
}