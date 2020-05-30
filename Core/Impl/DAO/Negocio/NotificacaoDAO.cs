using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Impl.DAO.Negocio
{
    class NotificacaoDAO : AbstractDAO
    {
        public NotificacaoDAO() : base("NotificacoesUsuarios", "NotificacaoId")
        {
        }
        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            Notificacao notificacao = (Notificacao)entidade;
            List<Notificacao> notificacoes;
            string cmdTextoNotificacao;

            try
            {
                Conectar();
                cmdTextoNotificacao = "SELECT * FROM NotificacoesUsuarios WHERE UsuarioId = @UsuarioId AND Visualizada = @Visualizada";

                SqlCommand comandoNotificacao = new SqlCommand(cmdTextoNotificacao, conexao);

                comandoNotificacao.Parameters.AddWithValue("@UsuarioId", notificacao.UsuarioId);
                comandoNotificacao.Parameters.AddWithValue("@Visualizada", notificacao.Visualizada);

                SqlDataReader drNotificacao = comandoNotificacao.ExecuteReader();
                comandoNotificacao.Dispose();

                notificacoes = DataReaderNotificacaoParaList(drNotificacao);
            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
            finally
            {
                Desconectar();
            }
            return notificacoes.ToList<EntidadeDominio>();
        }
        public override void Alterar(EntidadeDominio entidade)
        {
            Notificacao notificacao = (Notificacao)entidade;
            string cmdTextoNotificacao;

            try
            {
                Conectar();
                BeginTransaction();
                cmdTextoNotificacao = "UPDATE NotificacoesUsuarios SET Visualizada = @Visualizada WHERE NotificacaoId = @NotificacaoId";

                SqlCommand comandoNotificacao = new SqlCommand(cmdTextoNotificacao, conexao, transacao);

                comandoNotificacao.Parameters.AddWithValue("@Visualizada", notificacao.Visualizada);
                comandoNotificacao.Parameters.AddWithValue("@NotificacaoId", notificacao.Id);
                comandoNotificacao.ExecuteNonQuery();
                comandoNotificacao.Dispose();
                Commit();
            }
            catch (SqlException e)
            {
                Rollback();
                throw e;
            }
            catch (InvalidOperationException e)
            {
                Rollback();
                throw e;
            }
            finally
            {
                Desconectar();
            }
        }
        public List<Notificacao> DataReaderNotificacaoParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
                return new List<Notificacao>();

            List<Notificacao> notificacoes = new List<Notificacao>();
            while (dataReader.Read())
            {
                try
                {
                    Notificacao notificacao = new Notificacao
                    {
                        Id = Convert.ToInt32(dataReader["NotificacaoId"]),
                        UsuarioId = Convert.ToInt32(dataReader["UsuarioId"]),
                        Titulo = dataReader["Titulo"].ToString(),
                        Descricao = dataReader["Descricao"].ToString(),
                        Visualizada = Convert.ToByte(dataReader["Visualizada"]),
                        DataCadastro = Convert.ToDateTime(dataReader["Data"])
                    };
                    notificacoes.Add(notificacao);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return notificacoes.ToList();
        }
    }
}
