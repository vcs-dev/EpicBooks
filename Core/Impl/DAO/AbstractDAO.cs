using Core.Interfaces;
using Core.Util;
using Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace Core.Impl.DAO
{
    public abstract class AbstractDAO : IDAO
    {
        protected SqlConnection conexao;
        protected string tabela;
        protected string idTabela;
        protected SqlTransaction transacao;

        public AbstractDAO(SqlConnection conexao, string tabela, string idTabela)
        {
            this.tabela = tabela;
            this.idTabela = idTabela;
            this.conexao = conexao;
        }

        protected AbstractDAO(string tabela, string idTabela)
        {
            this.tabela = tabela;
            this.idTabela = idTabela;
        }

        public virtual void Salvar(EntidadeDominio entidade)
        {
            throw new NotImplementedException();
        }

        public virtual void Alterar(EntidadeDominio entidade)
        {
            throw new NotImplementedException();
        }

        public virtual List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            throw new NotImplementedException();
        }

        public virtual void Excluir(EntidadeDominio entidade)
        {
            try
            {
                Conectar();
                BeginTransaction();
                string cmdTexto = "DELETE FROM " + tabela + " WHERE = " + idTabela;
                SqlCommand comando = new SqlCommand(cmdTexto, conexao, transacao);
                comando.ExecuteNonQuery();
                Commit();
                comando.Dispose();
            }
            catch (DbException e)
            {
                Rollback();
                throw e;
            }
            finally
            {
                Desconectar();
            }
        }

        protected void Conectar()
        {
            try
            {
                if (conexao == null)
                {
                    conexao = ConexaoBd.GetConexao();
                    conexao.Open();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
        }

        protected void Desconectar()
        {
            if (conexao != null)
            {
                conexao.Close();
                conexao.Dispose();
            }
        }

        public void BeginTransaction()
        {
            try
            {
                transacao = conexao.BeginTransaction();
            }
            catch (DbException e)
            {
                throw e;
            }
        }

        public void Commit()
        {
            try
            {
                transacao.Commit();
            }
            catch (DbException e)
            {
                throw e;
            }
        }

        public void Rollback()
        {
            try
            {
                transacao.Rollback();
            }
            catch (DbException e)
            {
                throw e;
            }
        }
    }
}
