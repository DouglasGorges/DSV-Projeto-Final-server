using System;
using System.Collections.Generic;
using System.Linq;
using server.Data;
using server.Models;
using server.Controllers;

namespace server.Utils
{
    public class TransacaoUtil
    {
        private TransacaoController _transacaoController;
        private readonly DataContext _context;
        private readonly CategoriaUtil _categoriaUtil = new CategoriaUtil();
        public TransacaoUtil(DataContext context){
            _context = context;
            _transacaoController = new TransacaoController(_context);
        }

        public bool ExisteTransacaoComACategoria(Categoria categoria) {
            bool existeTransacaoComACategoria = false;

            List<Transacao> listaTransacoes = _context.Transacoes.ToList();

            listaTransacoes.ForEach(delegate(Transacao transacao) {
                transacao.Categorias.ToList().ForEach(delegate(Categoria itemCategoria){
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
                
                if((!listaTransacoesFiltradas.Contains(transacao)
                        && matchContaCorrente(transacao, filtro)
                        && matchCategorias(transacao.Categorias.ToList(), filtro)
                        && matchStatusTransacao(transacao, filtro)
                        && matchDtVcto(transacao.DataVencimento, filtro)
                        && matchDtPgto(transacao.DataPagamento, filtro))){
                    listaTransacoesFiltradas.Add(transacao);
                } 
            });

            return listaTransacoesFiltradas;
        }

        private bool matchContaCorrente(Transacao transacao, FiltroPesquisa filtro) => filtro.ContaCorrente == null || (transacao.ContaCorrente != null && transacao.ContaCorrente.Equals(filtro.ContaCorrente));

        private bool matchCategorias(List<Categoria> listaCategorias, FiltroPesquisa filtro){
            if(filtro.ListaCategorias == null || !filtro.ListaCategorias.Any()){
                return true;
            }

            return (listaCategorias != null && listaCategorias.Any() && _categoriaUtil.hasMatch(listaCategorias, filtro.ListaCategorias));
        }

        private bool matchStatusTransacao(Transacao transacao, FiltroPesquisa filtro){
            if(filtro.StatusTransacao == StatusTransacao.Todos){
                return true;
            }
        
            StatusTransacao statusTransacao;
            if(transacao.DataPagamento == null){
                statusTransacao = StatusTransacao.Nao_Pago;
                if(DateTime.Now > transacao.DataVencimento){
                    statusTransacao = StatusTransacao.Atrasado;
                }
            } else {
                statusTransacao = StatusTransacao.Pago;
            }

            return statusTransacao.Equals(filtro.StatusTransacao);
        }

        private bool matchDtVcto(DateTime dtVcto, FiltroPesquisa filtro){
            return (filtro.DtVctoInicial == null || dtVcto >= filtro.DtVctoInicial) && (filtro.DtVctoFinal == null || dtVcto <= filtro.DtVctoFinal);
        }

        private bool matchDtPgto(DateTime? dtPgto, FiltroPesquisa filtro){
            return (filtro.DtPgtoInicial == null || dtPgto >= filtro.DtPgtoInicial) && (filtro.DtPgtoFinal == null || dtPgto <= filtro.DtPgtoFinal);
        }
    }
}