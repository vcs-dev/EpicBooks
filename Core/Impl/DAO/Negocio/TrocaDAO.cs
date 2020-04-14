using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Core.Impl.DAO.Negocio
{
    public class TrocaDAO : AbstractDAO
    {
        public TrocaDAO() : base("Trocas", "TrocaId")
        {
        }

        public override void Salvar(EntidadeDominio entidade)
        {
            Troca troca = (Troca)entidade;
            string cmdTextoTroca;
            string cmdTextoPedido;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextoTroca = "INSERT INTO Trocas" +
                                    "(PedidoId," +
                                    "ItemId," +
                                    "Status," +
                                    "Qtde," +
                                    "DataSolicitacao" +
                                ") " +
                                "VALUES" +
                                    "(@PedidoId," +
                                    "@ItemId," +
                                    "@Status," +
                                    "@Qtde," +
                                    "@DataSolicitacao" +
                                ") SELECT CAST(scope_identity() AS int)";

                SqlCommand comandoTroca = new SqlCommand(cmdTextoTroca, conexao, transacao);

                comandoTroca.Parameters.AddWithValue("@PedidoId", troca.PedidoId);
                comandoTroca.Parameters.AddWithValue("@ItemId", troca.ItemId);
                comandoTroca.Parameters.AddWithValue("@Status", troca.Status);
                comandoTroca.Parameters.AddWithValue("@Qtde", troca.Qtde);
                comandoTroca.Parameters.AddWithValue("@DataSolicitacao", troca.DataCadastro);
                troca.Id = Convert.ToInt32(comandoTroca.ExecuteScalar());
                comandoTroca.Dispose();

                cmdTextoPedido = "UPDATE Pedidos SET Status = 'X' WHERE PedidoId = @PedidoId";

                SqlCommand comandoPedido = new SqlCommand(cmdTextoPedido, conexao, transacao);

                comandoPedido.Parameters.AddWithValue("@PedidoId", troca.PedidoId);
                comandoPedido.ExecuteNonQuery();
                comandoPedido.Dispose();

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

        public override void Alterar(EntidadeDominio entidade)
        {
            Troca troca = (Troca)entidade;
            string cmdTextoTroca;
            string cmdTextoEstoque;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextoTroca = "UPDATE Trocas SET Status = @Status " +
                                "WHERE PedidoId = @PedidoId AND ItemId = @ItemId";

                SqlCommand comandoTroca = new SqlCommand(cmdTextoTroca, conexao, transacao);

                comandoTroca.Parameters.AddWithValue("@Status", troca.Status);
                comandoTroca.Parameters.AddWithValue("@PedidoId", troca.PedidoId);
                comandoTroca.Parameters.AddWithValue("@ItemId", troca.ItemId);

                comandoTroca.ExecuteNonQuery();

                cmdTextoTroca = "INSERT INTO Cupons" +
                                    "(Codigo," +
                                    "Tipo," +
                                    "Valor," +
                                    "DataExpiracao," +
                                    "Usado," +
                                    "UsuarioId" +
                                 ") " +
                                 "VALUES" +
                                    "(@Codigo," +
                                    "@Tipo," +
                                    "@Valor," +
                                    "@DataExpiracao," +
                                    "@Usado," +
                                    "@UsuarioId" +
                                 ")";

                comandoTroca = new SqlCommand(cmdTextoTroca, conexao, transacao);

                comandoTroca.Parameters.AddWithValue("@Codigo", troca.CupomTroca.Codigo);
                comandoTroca.Parameters.AddWithValue("@Tipo", troca.CupomTroca.Tipo);
                comandoTroca.Parameters.AddWithValue("@Valor", troca.CupomTroca.Valor);
                if(troca.CupomTroca.DataExpiracao == null)
                    comandoTroca.Parameters.AddWithValue("@DataExpiracao", DBNull.Value);
                else
                    comandoTroca.Parameters.AddWithValue("@DataExpiracao", troca.CupomTroca.DataExpiracao);
                comandoTroca.Parameters.AddWithValue("@Usado", troca.CupomTroca.Usado);
                comandoTroca.Parameters.AddWithValue("@UsuarioId", troca.CupomTroca.UsuarioId);
                comandoTroca.ExecuteNonQuery();

                comandoTroca.Dispose();

                if (troca.VoltaParEstoque)
                {
                    cmdTextoEstoque = "INSERT INTO EntradaEstoque" +
                                         "(ProdutoId," +
                                         "Qtde," +
                                         "ValorCusto," +
                                         "DataEntrada," +
                                         "Observacao" +
                                      ") " +
                                      "VALUES" +
                                         "(@ProdutoId," +
                                         "@Qtde," +
                                         "(SELECT ValorCusto FROM Estoque WHERE ProdutoId = @ProdutoId)," +
                                         "@DataEntrada," +
                                         "@Observacao" +
                                 ")";

                    SqlCommand comandoEstoque = new SqlCommand(cmdTextoEstoque, conexao, transacao);

                    comandoEstoque.Parameters.AddWithValue("@ProdutoId", troca.ItemId);
                    comandoEstoque.Parameters.AddWithValue("@Qtde", troca.Qtde);
                    comandoEstoque.Parameters.AddWithValue("@DataEntrada", DateTime.Now);
                    comandoEstoque.Parameters.AddWithValue("@Observacao", troca.EstoqueObservacao);
                    comandoEstoque.ExecuteNonQuery();

                    cmdTextoEstoque = "UPDATE Estoque SET Qtde = Qtde + @Qtde WHERE ProdutoId = @ProdutoId";

                    comandoEstoque = new SqlCommand(cmdTextoEstoque, conexao, transacao);

                    comandoEstoque.Parameters.AddWithValue("@Qtde", troca.Qtde);
                    comandoEstoque.Parameters.AddWithValue("@ProdutoId", troca.ItemId);
                    comandoEstoque.ExecuteNonQuery();
                    comandoEstoque.Dispose();
                }
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
            catch (InvalidCastException e)
            {
                Rollback();
                throw e;
            }
            catch (IOException e)
            {
                Rollback();
                throw e;
            }
            catch (Exception e)
            {
                Rollback();
                throw e;
            }
            finally
            {
                Desconectar();
            }
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            Troca troca = (Troca)entidade;
            List<Troca> trocas = new List<Troca>();
            string cmdTextoTroca;

            try
            {
                Conectar();

                if (troca.UsuarioId > 0)
                    cmdTextoTroca = "SELECT " +
                                         "T.TrocaId, " +
                                         "P.UsuarioId," +
                                         "T.PedidoId, " +
                                         "T.ItemId, " +
                                         "PR.Nome, " +
                                         "T.Status, " +
                                         "T.Qtde, " +
                                         "T.DataSolicitacao " +
                                     "FROM Trocas T " +
                                         "JOIN Produtos PR ON(T.ItemId = PR.ProdutoId) " +
                                         "JOIN Pedidos P ON(T.PedidoId = P.PedidoId) " +
                                         "JOIN Usuarios U ON(P.UsuarioId = U.UsuarioId) " +
                                     "WHERE P.UsuarioId = @UsuarioId " +
                                     "ORDER BY TrocaId";
                else
                    cmdTextoTroca = "SELECT " +
                                         "T.TrocaId, " +
                                         "P.UsuarioId," +
                                         "T.PedidoId, " +
                                         "T.ItemId, " +
                                         "PR.Nome, " +
                                         "T.Qtde, " +
                                         "T.Status, " +
                                         "T.DataSolicitacao " +
                                     "FROM Trocas T " +
                                         "JOIN Produtos PR ON(T.ItemId = PR.ProdutoId) " +
                                         "JOIN Pedidos P ON(T.PedidoId = P.PedidoId) " +
                                     "WHERE T.Status = @Status " +
                                     "ORDER BY DataSolicitacao ASC";

                SqlCommand comandotroca = new SqlCommand(cmdTextoTroca, conexao);

                if (troca.UsuarioId > 0)
                    comandotroca.Parameters.AddWithValue("@UsuarioId", troca.UsuarioId);
                else if (troca.Status > 0)
                    comandotroca.Parameters.AddWithValue("@Status", troca.Status);

                SqlDataReader drTroca = comandotroca.ExecuteReader();
                comandotroca.Parameters.Clear();

                trocas = DataReaderTrocaParaList(drTroca);

                comandotroca.Dispose();
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
            return trocas.ToList<EntidadeDominio>();
        }

        public List<Troca> DataReaderTrocaParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
                return new List<Troca>();

            List<Troca> trocas = new List<Troca>();
            while (dataReader.Read())
            {
                try
                {
                    Troca troca = new Troca
                    {
                        Id = Convert.ToInt32(dataReader["TrocaId"]),
                        PedidoId = Convert.ToInt32(dataReader["PedidoId"]),
                        ItemId = Convert.ToInt32(dataReader["ItemId"]),
                        Status = Convert.ToChar(dataReader["Status"]),
                        Qtde = Convert.ToInt32(dataReader["Qtde"]),
                        DataCadastro = Convert.ToDateTime(dataReader["DataSolicitacao"]),
                    };
                    if (!Convert.IsDBNull(dataReader["Nome"]))
                        troca.NomeItem = (dataReader["Nome"]).ToString();
                    if (!Convert.IsDBNull(dataReader["UsuarioId"]))
                        troca.UsuarioId = Convert.ToInt32(dataReader["UsuarioId"]);

                    trocas.Add(troca);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return trocas.ToList();
        }
    }
}
