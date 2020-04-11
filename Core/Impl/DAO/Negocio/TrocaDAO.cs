using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

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
                comandoTroca.Parameters.AddWithValue("@DataExpiracao", troca.CupomTroca.DataExpiracao);
                comandoTroca.Parameters.AddWithValue("@Usado", troca.CupomTroca.Usado);
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
    }
}
