using System;
using System.Collections.Generic;

namespace server.Models
{
    public class CategoriaTransacao
    {
        public DateTime CriadoEm { get; set; }
        public int CategoriaId { get; set;}
        public Categoria Categoria { get; set;}
        public int TransacaoId { get; set;}
        public Transacao Transacao { get; set;}
   
    }
}