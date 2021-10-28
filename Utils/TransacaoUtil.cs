using System;
using System.Collections.Generic;
using System.Linq;
using server.Models;
using server.Controllers;

namespace server.Utils
{
    public class TransacaoUtil
    {
        private TransacaoController _transacaoController;
        private readonly CategoriaUtil _categoriaUtil = new CategoriaUtil();

        public bool ExisteTransacaoComACategoria(Categoria categoria) {
            bool existeTransacaoComACategoria = false;

            List<Transacao> listaTransacoes = _transacaoController.List();
            listaTransacoes.ForEach(delegate(Transacao transacao) {
                transacao.ListaCategorias.ForEach(delegate(Categoria itemCategoria){
                    if(itemCategoria.Equals(categoria)){
                        existeTransacaoComACategoria = true;
                    }
                });
            });

            return existeTransacaoComACategoria;
        }

        public bool ExisteTransacaoComAContaCorrente(ContaCorrente contaCorrente) {
            bool existeTransacaoComAContaCorrente = false;

            List<Transacao> listaTransacoes = _transacaoController.List();
            listaTransacoes.ForEach(delegate(Transacao transacao) {
                if(transacao.ContaCorrente.Equals(contaCorrente)){
                    existeTransacaoComAContaCorrente = true;
                }
            });

            return existeTransacaoComAContaCorrente;
        }

        public List<Transacao> buscarTransacoesFiltradas(FiltroPesquisa filtro){
            List<Transacao> listaTransacoes = _transacaoController.List();
            List<Transacao> listaTransacoesFiltradas = new List<Transacao>();

            listaTransacoes.ForEach(delegate(Transacao transacao){
                if(matchContaCorrente(transacao, filtro)
                        || matchCategorias(transacao.ListaCategorias, filtro)
                        || matchStatusTransacao(transacao, filtro)
                        || matchDtVcto(transacao.DataVencimento, filtro)
                        || matchDtPgto(transacao.DataPagamento, filtro)){
                    listaTransacoesFiltradas.Add(transacao);
                } 
            });

            return listaTransacoesFiltradas;
        }

        private bool matchContaCorrente(Transacao transacao, FiltroPesquisa filtro){
            return (transacao.ContaCorrente != null && transacao.ContaCorrente.Equals(filtro.ContaCorrente));
        }

        private bool matchCategorias(List<Categoria> listaCategorias, FiltroPesquisa filtro){
            return (listaCategorias != null && listaCategorias.Any() && _categoriaUtil.hasMatch(listaCategorias, filtro.ListaCategorias));
        }

        private bool matchStatusTransacao(Transacao transacao, FiltroPesquisa filtro){
            StatusTransacao statusTransacao;
            
            if(transacao.DataPagamento == null){
                statusTransacao = StatusTransacao.Nao_Pago;
                if(DateTime.Now > transacao.DataVencimento){
                    statusTransacao = StatusTransacao.Atrasado;
                }
            } else {
                statusTransacao = StatusTransacao.Pago;
            }

            return statusTransacao.Equals(filtro.StatusTransacao) || filtro.StatusTransacao.Equals(StatusTransacao.Todos);
        }

        private bool matchDtVcto(DateTime dtVcto, FiltroPesquisa filtro){
            return (filtro.DtVctoInicial == null || dtVcto >= filtro.DtVctoInicial) && (filtro.DtVctoFinal == null || dtVcto <= filtro.DtVctoFinal);
        }

        private bool matchDtPgto(DateTime dtPgto, FiltroPesquisa filtro){
            return (filtro.DtPgtoInicial == null || dtPgto >= filtro.DtPgtoInicial) && (filtro.DtPgtoFinal == null || dtPgto <= filtro.DtPgtoFinal);
        }
    }
}