using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Impl.DAO.Negocio
{
    public class PedidoDAO : AbstractDAO
    {
        public PedidoDAO():base("Pedidos", "PedidoId")
        {
        }

        public override void Salvar(EntidadeDominio entidade)
        {
            Pedido pedido = (Pedido)entidade;
            string cmdTextoPedido;
            string cmdTextoPedidosItens;
            string cmdTextoPedidosCartoes;
            string cmdTextoSaidaEstoque;
            string cmdTextoEstoque;
            string cmdTextoBloqueados;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextoPedido = "INSERT INTO Pedidos(UsuarioId," +
                                                   "DataPedido," +
                                                   "Status" +
                                  ") " +
                                  "VALUES(@UsuarioId," +
                                         "@DataPedido," +
                                         "@Status" +
                                  ") SELECT CAST(scope_identity() AS int)";

                SqlCommand comandoPedido = new SqlCommand(cmdTextoPedido, conexao, transacao);

                comandoPedido.Parameters.AddWithValue("@UsuarioId", pedido.UsuarioId);
                comandoPedido.Parameters.AddWithValue("@DataPedido", pedido.DataCadastro);
                comandoPedido.Parameters.AddWithValue("@Status", pedido.Status);
                pedido.Id = Convert.ToInt32(comandoPedido.ExecuteScalar());
                comandoPedido.Dispose();

                cmdTextoPedidosItens = "INSERT INTO PedidosItens(PedidoId," +
                                                             "ItemId," +
                                                             "Qtde " +
                                       ") " +
                                       "VALUES(@PedidoId," +
                                             "@ItemId," +
                                             "@Qtde" +
                                       ")";
                SqlCommand comandoPedidosItens = new SqlCommand(cmdTextoPedidosItens, conexao, transacao);
                foreach (var item in pedido.ItensPedido)
                {
                    comandoPedidosItens.Parameters.AddWithValue("@PedidoId", pedido.Id);
                    comandoPedidosItens.Parameters.AddWithValue("@ItemId", item.Produto.Id);
                    comandoPedidosItens.Parameters.AddWithValue("@Qtde", item.Qtde);
                    comandoPedidosItens.ExecuteNonQuery();
                    comandoPedidosItens.Parameters.Clear();
                }
                comandoPedidosItens.Dispose();
                if (pedido.Status == 'A')
                {
                    cmdTextoPedidosCartoes = "INSERT INTO PedidosCartoes( " +
                                                                        "PedidoId," +
                                                                        "CartaoId," +
                                                                        "Valor" +
                                             ") " +
                                             "Values(" +
                                                    "@PedidoId," +
                                                    "@CartaoId," +
                                                    "@Valor," +
                                                    "@Parcelas" +
                                             ")";

                    SqlCommand comandoPedidosCartoes = new SqlCommand(cmdTextoPedidosCartoes, conexao, transacao);

                    comandoPedidosCartoes.Parameters.AddWithValue("@PedidoId", pedido.Id);
                    comandoPedidosCartoes.Parameters.AddWithValue("@CartaoId", pedido.CartaoUm);
                    if(pedido.CartaoUm.Valor != null)
                        comandoPedidosCartoes.Parameters.AddWithValue("@Valor", pedido.CartaoUm.Valor);
                    comandoPedidosCartoes.Parameters.AddWithValue("@Parcelas", pedido.CartaoUm.QtdeParcelas);
                    comandoPedidosCartoes.ExecuteNonQuery();
                    comandoPedidosCartoes.Parameters.Clear();

                    if (pedido.CartaoDois.Id > 0)
                    {
                        comandoPedidosCartoes.Parameters.AddWithValue("@PedidoId", pedido.Id);
                        comandoPedidosCartoes.Parameters.AddWithValue("@CartaoId", pedido.CartaoDois);
                        comandoPedidosCartoes.Parameters.AddWithValue("@Valor", pedido.CartaoDois.Valor);
                        comandoPedidosCartoes.Parameters.AddWithValue("@Parcelas", pedido.CartaoDois.QtdeParcelas);
                        comandoPedidosCartoes.ExecuteNonQuery();
                    }

                    comandoPedidosCartoes.Dispose();

                    cmdTextoSaidaEstoque = "INSERT INTO SaidaEstoque(ProdutoId," +
                                                                   "Qtde," +
                                                                   "PedidoId," +
                                                                   "DataSaida" +
                                           ") " +
                                           "Values(" +
                                                                   "@ProdutoId," +
                                                                   "@Qtde," +
                                                                   "@PedidoId," +
                                                                   "@DataSaida" +
                                           ")";

                    SqlCommand comandoSaidaEstoque = new SqlCommand(cmdTextoSaidaEstoque, conexao, transacao);
                    foreach (var item in pedido.ItensPedido)
                    {
                        comandoSaidaEstoque.Parameters.AddWithValue("@ProdutoId", item.Produto.Id);
                        comandoSaidaEstoque.Parameters.AddWithValue("@Qtde", item.Qtde);
                        comandoSaidaEstoque.Parameters.AddWithValue("@PedidoId", pedido.Id);
                        comandoSaidaEstoque.Parameters.AddWithValue("@DataSaida", DateTime.Now);
                        comandoSaidaEstoque.ExecuteNonQuery();
                        comandoSaidaEstoque.Parameters.Clear();
                    }
                    comandoSaidaEstoque.Dispose();

                    cmdTextoEstoque = "Update Estoque " +
                                      "SET Qtde = ( " +
                                                     "SELECT Qtde FROM Estoque " +
                                                     "WHERE ProdutoId  = @ProdutoId " +
                                                " ) - @Qtde " +
                                      "WHERE ProdutoId = @ProdutoId";
                    SqlCommand comandoEstoque = new SqlCommand(cmdTextoEstoque, conexao, transacao);
                    foreach (var item in pedido.ItensPedido)
                    {
                        comandoEstoque.Parameters.AddWithValue("@ProdutoId", item.Produto.Id);
                        comandoEstoque.Parameters.AddWithValue("@Qtde", item.Qtde);
                        comandoEstoque.ExecuteNonQuery();
                        comandoEstoque.Parameters.Clear();
                    }
                    comandoEstoque.Dispose();
                }

                cmdTextoBloqueados = "DELETE FROM ItensBloqueados " +
                                     "WHERE SessaoGuid = @SessaoGuid";
                SqlCommand comandoBloqueados = new SqlCommand(cmdTextoBloqueados, conexao, transacao);
                foreach (var item in pedido.ItensPedido)
                {
                    comandoBloqueados.Parameters.AddWithValue("@SessaoGuid", pedido.SessaoGuid);
                    comandoBloqueados.ExecuteNonQuery();
                    comandoBloqueados.Parameters.Clear();
                }
                comandoBloqueados.Dispose();
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

    }
}
