using System;
using System.Collections.Generic;

namespace server.Models
{
    public class FiltroPesquisa
    {
        public FiltroPesquisa() => StatusTransacao = StatusTransacao.Todos;
        public FiltroPesquisa(ContaCorrente contaCorrente, StatusTransacao status)
        {
            ContaCorrente = contaCorrente;
            StatusTransacao = status;
        }

        public ContaCorrente ContaCorrente { get; set; }
        public List<Categoria> ListaCategorias { get; set; }
        public StatusTransacao StatusTransacao { get; set; }
        public DateTime? DtVctoInicial { get; set; }
        public DateTime? DtVctoFinal { get; set; }
        public DateTime? DtPgtoInicial { get; set; }
        public DateTime? DtPgtoFinal { get; set; }

    }
}