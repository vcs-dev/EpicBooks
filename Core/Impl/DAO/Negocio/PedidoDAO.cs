using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Core.Impl.DAO.Negocio
{
    public class PedidoDAO : AbstractDAO
    {
        public PedidoDAO() : base("Pedidos", "PedidoId")
        {
        }

        public override void Salvar(EntidadeDominio entidade)
        {
            Pedido pedido = (Pedido)entidade;
            string cmdTextoPedido;
            string cmdTextoPedidosItens;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextoPedido = "INSERT INTO Pedidos(UsuarioId, " +
                                                   "EnderecoId, " +
                                                   "ValorFrete, " +
                                                   "DataPedido, " +
                                                   "Status, " +
                                                   "Observacao" +
                                  ") " +
                                  "VALUES(@UsuarioId, " +
                                         "@EnderecoId, " +
                                         "@ValorFrete, " +
                                         "@DataPedido, " +
                                         "@Status," +
                                         "@Observacao" +
                                  ") SELECT CAST(scope_identity() AS int)";

                SqlCommand comandoPedido = new SqlCommand(cmdTextoPedido, conexao, transacao);

                comandoPedido.Parameters.AddWithValue("@UsuarioId", pedido.UsuarioId);
                comandoPedido.Parameters.AddWithValue("@EnderecoId", pedido.EnderecoId);
                comandoPedido.Parameters.AddWithValue("@ValorFrete", pedido.ValorFrete);
                comandoPedido.Parameters.AddWithValue("@DataPedido", pedido.DataCadastro);
                if (string.IsNullOrEmpty(pedido.Observacao) || string.IsNullOrWhiteSpace(pedido.Observacao))
                    comandoPedido.Parameters.AddWithValue("@Observacao", DBNull.Value);
                else
                    comandoPedido.Parameters.AddWithValue("@Observacao", pedido.Observacao);
                comandoPedido.Parameters.AddWithValue("@Status", pedido.Status);
                pedido.Id = Convert.ToInt32(comandoPedido.ExecuteScalar());
                comandoPedido.Dispose();

                cmdTextoPedidosItens = "INSERT INTO PedidosItens(PedidoId, " +
                                                             "ItemId, " +
                                                             "Qtde, " +
                                                             "PrecoUnitario" +
                                       ") " +
                                       "VALUES(@PedidoId, " +
                                             "@ItemId, " +
                                             "@Qtde, " +
                                             "@PrecoUnitario" +
                                       ")";
                SqlCommand comandoPedidosItens = new SqlCommand(cmdTextoPedidosItens, conexao, transacao);
                foreach (var item in pedido.ItensPedido)
                {
                    comandoPedidosItens.Parameters.AddWithValue("@PedidoId", pedido.Id);
                    comandoPedidosItens.Parameters.AddWithValue("@ItemId", item.Produto.Id);
                    comandoPedidosItens.Parameters.AddWithValue("@Qtde", item.Qtde);
                    comandoPedidosItens.Parameters.AddWithValue("@PrecoUnitario", item.Produto.PrecoVenda);
                    comandoPedidosItens.ExecuteNonQuery();
                    comandoPedidosItens.Parameters.Clear();
                }
                comandoPedidosItens.Dispose();
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
            Pedido pedido = (Pedido)entidade;
            string cmdTextoPedido;
            string cmdTextoPedidosCupons;
            string cmdTextoVenda;
            string cmdTextoEstoque;
            string cmdTextoPedidosCartoes;
            string cmdTextoBloqueados;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextoPedido = "UPDATE Pedidos SET Status = @Status WHERE PedidoId = @PedidoId";

                SqlCommand comandopedido = new SqlCommand(cmdTextoPedido, conexao, transacao);

                comandopedido.Parameters.AddWithValue("@Status", pedido.Status);
                comandopedido.Parameters.AddWithValue("@PedidoId", pedido.Id);

                comandopedido.ExecuteNonQuery();

                if (pedido.Status == 'A')
                {
                    cmdTextoPedidosCartoes = "INSERT INTO PedidosCartoes(" +
                                                                        "PedidoId, " +
                                                                        "CartaoId, " +
                                                                        "Valor, " +
                                                                        "Parcelas" +
                                             ") " +
                                             "Values(" +
                                                    "@PedidoId, " +
                                                    "@CartaoId, " +
                                                    "@Valor, " +
                                                    "@Parcelas" +
                                             ")";

                    SqlCommand comandoPedidosCartoes = new SqlCommand(cmdTextoPedidosCartoes, conexao, transacao);

                    comandoPedidosCartoes.Parameters.AddWithValue("@PedidoId", pedido.Id);
                    comandoPedidosCartoes.Parameters.AddWithValue("@CartaoId", pedido.CartaoUm.Id);
                    if (pedido.CartaoUm.Valor != null)
                        comandoPedidosCartoes.Parameters.AddWithValue("@Valor", pedido.CartaoUm.Valor);
                    comandoPedidosCartoes.Parameters.AddWithValue("@Parcelas", pedido.CartaoUm.QtdeParcelas);
                    comandoPedidosCartoes.ExecuteNonQuery();
                    comandoPedidosCartoes.Parameters.Clear();

                    if (pedido.CartaoDois.Id > 0)
                    {
                        comandoPedidosCartoes.Parameters.AddWithValue("@PedidoId", pedido.Id);
                        comandoPedidosCartoes.Parameters.AddWithValue("@CartaoId", pedido.CartaoDois.Id);
                        comandoPedidosCartoes.Parameters.AddWithValue("@Valor", pedido.CartaoDois.Valor);
                        comandoPedidosCartoes.Parameters.AddWithValue("@Parcelas", pedido.CartaoDois.QtdeParcelas);
                        comandoPedidosCartoes.ExecuteNonQuery();
                    }

                    comandoPedidosCartoes.Dispose();

                    if (pedido.CuponsTroca.Count > 0 || pedido.CupomPromocional.Id > 0)
                    {
                        cmdTextoPedidosCupons = "INSERT INTO PedidosCupons(PedidoId, " +
                                                             "CupomId" +
                                                ") " +
                                                "VALUES(@PedidoId, " +
                                                      "@CupomId" +
                                                ")";

                        SqlCommand comandoPedidosCupons = new SqlCommand(cmdTextoPedidosCupons, conexao, transacao);
                        if (pedido.CuponsTroca.Count > 0)
                        {
                            foreach (var item in pedido.CuponsTroca)
                            {
                                comandoPedidosCupons.Parameters.AddWithValue("@PedidoId", pedido.Id);
                                comandoPedidosCupons.Parameters.AddWithValue("@CupomId", item.Id);
                                comandoPedidosCupons.ExecuteNonQuery();
                                comandoPedidosCupons.Parameters.Clear();
                            }
                        }
                        if (pedido.CupomPromocional.Id > 0)
                        {
                            comandoPedidosCupons.Parameters.AddWithValue("@PedidoId", pedido.Id);
                            comandoPedidosCupons.Parameters.AddWithValue("@CupomId", pedido.CupomPromocional.Id);
                            comandoPedidosCupons.ExecuteNonQuery();
                            comandoPedidosCupons.Parameters.Clear();
                        }

                        cmdTextoPedidosCupons = "UPDATE Cupons SET Usado = 1 WHERE CupomId = @CupomId";

                        comandoPedidosCupons = new SqlCommand(cmdTextoPedidosCupons, conexao, transacao);
                        if (pedido.CuponsTroca.Count > 0)
                        {
                            foreach (var item in pedido.CuponsTroca)
                            {
                                comandoPedidosCupons.Parameters.AddWithValue("@CupomId", item.Id);
                                comandoPedidosCupons.ExecuteNonQuery();
                                comandoPedidosCupons.Parameters.Clear();
                            }
                        }

                        comandoPedidosCupons.Dispose();

                        //verificar o codigo abaixo
                        //cmdTextoBloqueados = "DELETE FROM ItensBloqueados WHERE SessaoGuid = @SessaoGuid";

                        //SqlCommand comandoBloqueados = new SqlCommand(cmdTextoBloqueados, conexao, transacao);
                        //foreach (var item in pedido.ItensPedido)
                        //{
                        //    comandoBloqueados.Parameters.AddWithValue("@SessaoGuid", pedido.SessaoGuid);
                        //    comandoBloqueados.ExecuteNonQuery();
                        //    comandoBloqueados.Parameters.Clear();
                        //}
                        //comandoBloqueados.Dispose();
                    }

                    cmdTextoVenda = "INSERT INTO Vendas(ProdutoId, " +
                                                       "Qtde, " +
                                                       "PedidoId, " +
                                                       "DataVenda" +
                                           ") " +
                                           "Values(" +
                                                 "@ProdutoId, " +
                                                 "@Qtde, " +
                                                 "@PedidoId, " +
                                                 "@DataVenda" +
                                           ")";

                    SqlCommand comandoSaidaEstoque = new SqlCommand(cmdTextoVenda, conexao, transacao);
                    foreach (var item in pedido.ItensPedido)
                    {
                        comandoSaidaEstoque.Parameters.AddWithValue("@ProdutoId", item.Produto.Id);
                        comandoSaidaEstoque.Parameters.AddWithValue("@Qtde", item.Qtde);
                        comandoSaidaEstoque.Parameters.AddWithValue("@PedidoId", pedido.Id);
                        comandoSaidaEstoque.Parameters.AddWithValue("@DataVenda", DateTime.Now);
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

                Commit();
                comandopedido.Dispose();
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
            Pedido pedido = (Pedido)entidade;
            List<Pedido> pedidos = new List<Pedido>();
            string cmdTextoPedido;

            try
            {
                Conectar();

                if (pedido.Id > 0)
                    cmdTextoPedido = "SELECT * FROM Pedidos WHERE PedidoId = @PedidoId";
                if (pedido.Status != '\0')
                    cmdTextoPedido = "SELECT * FROM Pedidos WHERE Status = @Status";
                else
                    cmdTextoPedido = "SELECT * FROM Pedidos";

                SqlCommand comandopedido = new SqlCommand(cmdTextoPedido, conexao);

                if (pedido.Id > 0)
                    comandopedido.Parameters.AddWithValue("@PedidoId", pedido.Id);
                else if (pedido.Status != '\0')
                    comandopedido.Parameters.AddWithValue("@Status", pedido.Status);

                SqlDataReader drPedido = comandopedido.ExecuteReader();
                comandopedido.Parameters.Clear();

                pedidos = DataReaderPedidoParaList(drPedido);

                if (pedido.Status != 'A')
                {
                    foreach (var item in pedidos)
                    {
                        cmdTextoPedido = "SELECT ItemId, Qtde, PrecoUnitario" +
                                         "FROM Pedidos P " +
                                         "JOIN PedidosItens PI ON(P.PedidoId = PI.PedidoId) " +
                                         "WHERE P.PedidoId = @PedidoId";

                        comandopedido = new SqlCommand(cmdTextoPedido, conexao);

                        comandopedido.Parameters.AddWithValue("@PedidoId", item.Id);
                        drPedido = comandopedido.ExecuteReader();

                        if (!drPedido.HasRows)
                        {
                            throw new Exception("Sem Registros");
                        }

                        while (drPedido.Read())
                        {
                            ItemPedido temp = new ItemPedido
                            {
                                Id = drPedido.GetInt32(0),
                                Qtde = drPedido.GetInt32(1),
                                Produto = new Domain.Produto.Livro { PrecoVenda = Convert.ToDouble(drPedido.GetDecimal(2))}
                            };
                        }
                        drPedido.Close();
                    }
                }
                comandopedido.Dispose();
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
            return pedidos.ToList<EntidadeDominio>();
        }
        public List<Pedido> DataReaderPedidoParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
            {
                throw new Exception("Sem Registros");
            }

            List<Pedido> pedidos = new List<Pedido>();
            while (dataReader.Read())
            {
                try
                {
                    Pedido pedido = new Pedido
                    {
                        Id = Convert.ToInt32(dataReader["UsuarioId"]),
                        EnderecoId = Convert.ToInt32(dataReader["EnderecoId"]),
                        ValorFrete = Convert.ToDouble(dataReader["ValorFrete"]),
                        DataCadastro = Convert.ToDateTime(dataReader["DataPedido"]),
                        Status = Convert.ToChar(dataReader["Status"]),
                    };
                    if (!dataReader.IsDBNull(6))
                        pedido.Observacao = dataReader["Observacao"].ToString();

                    pedidos.Add(pedido);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return pedidos.ToList();
        }
    }
}
