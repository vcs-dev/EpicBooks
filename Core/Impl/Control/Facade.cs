using Core.Application;
using Core.Impl.Business;
using Core.Impl.DAO.Negocio;
using Core.Impl.DAO.Produto;
using Core.Interfaces;
using Domain;
using Domain.Negocio;
using Domain.Produto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Impl.Control
{
    public class Facade : IFacade
    {
        private readonly Dictionary<string, IDAO> daos;
        private readonly Dictionary<string, Dictionary<string, List<IStrategy>>> rns;
        private Result resultado;

        public Facade()
        {
            daos = new Dictionary<string, IDAO>();
            rns = new Dictionary<string, Dictionary<string, List<IStrategy>>>();

            #region Produto 
            ProdutoDAO produtoDAO = new ProdutoDAO();

            daos.Add(nameof(Livro), produtoDAO);

            //Regras de salvar
            ValidadorDadosObrigatoriosProduto validadorDadosObrgProd = new ValidadorDadosObrigatoriosProduto();
            InclusaoDataCadastro inclusaoDataCadastro = new InclusaoDataCadastro();
            //Regra genérica
            ValidadorIsbn validadorIsbn = new ValidadorIsbn();
            //Regras de alterar
            ValidadorDadosObrigatoriosProdutoEdicao validadorDadosObrgProdEdicao = new ValidadorDadosObrigatoriosProdutoEdicao();

            List<IStrategy> rnsSalvarProduto = new List<IStrategy>();
            List<IStrategy> rnsAlterarProduto = new List<IStrategy>();

            rnsSalvarProduto.Add(validadorDadosObrgProd);
            rnsSalvarProduto.Add(inclusaoDataCadastro);

            rnsAlterarProduto.Add(validadorDadosObrgProdEdicao);
            rnsAlterarProduto.Add(validadorIsbn);

            Dictionary<string, List<IStrategy>> rnsProduto = new Dictionary<string, List<IStrategy>>();

            rnsProduto.Add("SALVAR", rnsSalvarProduto);
            rnsProduto.Add("ALTERAR", rnsAlterarProduto);

            rns.Add(nameof(Livro), rnsProduto);
            #endregion

            #region Cupom
            CupomDAO cupomDAO = new CupomDAO();

            daos.Add(nameof(Cupom), produtoDAO);

            rns.Add(nameof(Cupom), null);
            #endregion

        }
        public Result Alterar(EntidadeDominio entidade)
        {
            resultado = new Result();
            string nmClasse = entidade.GetType().Name;
            string msg = ExecutarRegras(entidade, "ALTERAR");

            if (msg == null)
            {
                IDAO dao;
                daos.TryGetValue(nmClasse, out dao);
                try
                {
                    dao.Alterar(entidade);
                    List<EntidadeDominio> entidades = new List<EntidadeDominio>();
                    entidades.Add(entidade);
                    resultado.Entidades = entidades;
                }
                catch (Exception e)
                {
                    resultado.Msg = e.Message;
                }
            }
            else
            {
                resultado.Msg = msg;
            }
            return resultado;
        }

        public Result Consultar(EntidadeDominio entidade)
        {
            resultado = new Result();
            string nmClasse = entidade.GetType().Name;

            IDAO dao;
            daos.TryGetValue(nmClasse, out dao);
            try
            {
                resultado.Entidades = dao.Consultar(entidade);
            }
            catch (Exception e)
            {
                resultado.Msg = e.Message;
            }

            return resultado;
        }

        public Result Excluir(EntidadeDominio entidade)
        {
            resultado = new Result();
            string nmClasse = entidade.GetType().Name;
            string msg = ExecutarRegras(entidade, "EXCLUIR");

            if (msg == null)
            {
                IDAO dao;
                daos.TryGetValue(nmClasse, out dao);
                try
                {
                    dao.Excluir(entidade);
                    List<EntidadeDominio> entidades = new List<EntidadeDominio>();
                    entidades.Add(entidade);
                    resultado.Entidades = entidades;
                }
                catch (Exception e)
                {
                    resultado.Msg = e.Message;
                }
            }
            else
            {
                resultado.Msg = msg;
            }
            return resultado;
        }

        public Result Salvar(EntidadeDominio entidade)
        {
            resultado = new Result();
            string nmClasse = entidade.GetType().Name;
            string msg = ExecutarRegras(entidade, "SALVAR");

            if (msg == null)
            {
                IDAO dao;
                daos.TryGetValue(nmClasse, out dao);
                try
                {
                    dao.Salvar(entidade);
                    List<EntidadeDominio> entidades = new List<EntidadeDominio>();
                    entidades.Add(entidade);
                    resultado.Entidades = entidades;
                }
                catch (Exception e)
                {
                    resultado.Msg = e.Message;
                }
            }
            else
            {
                resultado.Msg = msg;
            }
            return resultado;
        }

        //public Result Visualizar(EntidadeDominio entidade)
        //{
        //    resultado = new Result();
        //    resultado.Entidades = new List<EntidadeDominio>(1);
        //    resultado.Entidades.Add(entidade);
        //    return resultado;
        //}

        private string ExecutarRegras(EntidadeDominio entidade, string operacao)
        {
            string nmClasse = entidade.GetType().Name;
            StringBuilder msg = new StringBuilder();

            Dictionary<string, List<IStrategy>> regrasOperacao;
            rns.TryGetValue(nmClasse, out regrasOperacao);

            if (regrasOperacao != null)
            {
                List<IStrategy> regras;
                regrasOperacao.TryGetValue(operacao, out regras);

                if (regras != null)
                {
                    foreach (var s in regras)
                    {
                        string m = s.Processar(entidade);

                        if (m != null)
                        {
                            msg.Append(m);
                            msg.Append("\n");
                        }
                    }
                }
            }

            if (msg.Length > 0)
                return msg.ToString();
            else
                return null;
        }
    }
}
