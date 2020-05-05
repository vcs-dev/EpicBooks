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
                                                   "DataPedido, " +
                                                   "Status, " +
                                                   "Observacao" +
                                  ") " +
                                  "VALUES(@UsuarioId, " +
                                         "@EnderecoId, " +
                                         "@DataPedido, " +
                                         "@Status," +
                                         "@Observacao" +
                                  ") SELECT CAST(scope_identity() AS int)";

                SqlCommand comandoPedido = new SqlCommand(cmdTextoPedido, conexao, transacao);

                comandoPedido.Parameters.AddWithValue("@UsuarioId", pedido.UsuarioId);
                comandoPedido.Parameters.AddWithValue("@EnderecoId", pedido.EnderecoId);
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
                if (!pedido.IsAlteracaoGerencial)
                {
                    if (pedido.Status == 'A')
                        cmdTextoPedido = "UPDATE Pedidos SET Status = @Status, ValorFrete = @ValorFrete WHERE PedidoId = @PedidoId";
                    else
                        cmdTextoPedido = "UPDATE Pedidos SET Status = @Status WHERE PedidoId = @PedidoId";

                    SqlCommand comandoPedido = new SqlCommand(cmdTextoPedido, conexao, transacao);

                    comandoPedido.Parameters.AddWithValue("@Status", pedido.Status);
                    if (pedido.Status == 'A')
                        comandoPedido.Parameters.AddWithValue("@ValorFrete", pedido.ValorFrete);
                    comandoPedido.Parameters.AddWithValue("@PedidoId", pedido.Id);

                    comandoPedido.ExecuteNonQuery();
                    comandoPedido.Dispose();

                    if (pedido.Status == 'A')
                    {
                        if (pedido.CartaoUm.Id > 0)
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
                            else
                                comandoPedidosCartoes.Parameters.AddWithValue("@Valor", DBNull.Value);
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
                        }

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

                            if (pedido.CupomTrocaGerado.Codigo != null && pedido.CupomTrocaGerado.Valor > 0)
                            {
                                cmdTextoPedidosCupons = "INSERT INTO Cupons(Codigo, Tipo, Valor, DataExpiracao, Usado, UsuarioId) " +
                                                        "VALUES(@Codigo, @Tipo, @Valor, @DataExpiracao, @Usado, @UsuarioId) SELECT CAST(scope_identity() AS int)";

                                comandoPedidosCupons = new SqlCommand(cmdTextoPedidosCupons, conexao, transacao);
                                comandoPedidosCupons.Parameters.AddWithValue("@Codigo", pedido.CupomTrocaGerado.Codigo + pedido.Id);
                                comandoPedidosCupons.Parameters.AddWithValue("@Tipo", pedido.CupomTrocaGerado.Tipo);
                                comandoPedidosCupons.Parameters.AddWithValue("@Valor", pedido.CupomTrocaGerado.Valor);
                                if (pedido.CupomTrocaGerado.DataExpiracao == null)
                                    comandoPedidosCupons.Parameters.AddWithValue("@DataExpiracao", DBNull.Value);
                                else
                                    comandoPedidosCupons.Parameters.AddWithValue("@DataExpiracao", pedido.CupomTrocaGerado.DataExpiracao);
                                comandoPedidosCupons.Parameters.AddWithValue("@Usado", pedido.CupomTrocaGerado.Usado);
                                comandoPedidosCupons.Parameters.AddWithValue("@UsuarioId", pedido.CupomTrocaGerado.UsuarioId);
                                pedido.CupomTrocaGerado.Id = Convert.ToInt32(comandoPedidosCupons.ExecuteScalar());
                                comandoPedidosCupons.Dispose();
                            }
                        }

                        cmdTextoVenda = "INSERT INTO Vendas(PedidoId, " +
                                                           "DataVenda" +
                                               ") " +
                                               "Values(@PedidoId, " +
                                                     "@DataVenda" +
                                               ")";

                        SqlCommand comandoVenda = new SqlCommand(cmdTextoVenda, conexao, transacao);

                        comandoVenda.Parameters.AddWithValue("@PedidoId", pedido.Id);
                        comandoVenda.Parameters.AddWithValue("@DataVenda", DateTime.Now);
                        comandoVenda.ExecuteNonQuery();
                        comandoVenda.Parameters.Clear();
                        comandoVenda.Dispose();

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
                    if (pedido.Status == 'A' || pedido.Status == 'R')
                    {
                        cmdTextoBloqueados = "DELETE FROM ItensBloqueados WHERE ItemId = @ItemId";

                        SqlCommand comandoBloqueados = new SqlCommand(cmdTextoBloqueados, conexao, transacao);
                        foreach (var item in pedido.ItensPedido)
                        {
                            comandoBloqueados.Parameters.AddWithValue("@ItemId", item.Produto.Id);
                            comandoBloqueados.ExecuteNonQuery();
                            comandoBloqueados.Parameters.Clear();
                        }
                        comandoBloqueados.Dispose();
                    }
                    comandoPedido.Dispose();
                }
                else
                {
                    cmdTextoPedido = "UPDATE Pedidos SET Status = @Status WHERE PedidoId = @PedidoId";

                    SqlCommand comandoPedido = new SqlCommand(cmdTextoPedido, conexao, transacao);
                    comandoPedido.Parameters.AddWithValue("@Status", pedido.Status);
                    comandoPedido.Parameters.AddWithValue("@PedidoId", pedido.Id);
                    comandoPedido.ExecuteNonQuery();
                    comandoPedido.Dispose();
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
            Pedido pedido = (Pedido)entidade;
            List<Pedido> pedidos = new List<Pedido>();
            string cmdTextoPedido;

            try
            {
                Conectar();

                if (pedido.UsuarioId > 0)
                    cmdTextoPedido = "SELECT " +
                                         "PedidoId, " +
                                         "UsuarioId, " +
                                         "EnderecoId, " +
                                         "ValorFrete, " +
                                         "DataPedido, " +
                                         "P.Status, " +
                                         "Observacao, " +
                                         "SPD.Descricao " +
                                     "FROM Pedidos P " +
                                         "JOIN StatusDePedidos SPD ON(P.Status = SPD.Status) " +
                                     "WHERE UsuarioId = @UsuarioId " +
                                     "ORDER BY PedidoId";
                else if (pedido.Id > 0)
                    cmdTextoPedido = "SELECT " +
                                         "PedidoId, " +
                                         "UsuarioId, " +
                                         "EnderecoId, " +
                                         "ValorFrete, " +
                                         "DataPedido, " +
                                         "P.Status, " +
                                         "Observacao, " +
                                         "SPD.Descricao " +
                                      "FROM Pedidos P " +
                                         "JOIN StatusDePedidos SPD ON(P.Status = SPD.Status) " +
                                      "WHERE PedidoId = @PedidoId " +
                                      "ORDER BY PedidoId";
                else if (pedido.Status != '\0')
                    cmdTextoPedido = "SELECT " +
                                         "PedidoId, " +
                                         "UsuarioId, " +
                                         "EnderecoId, " +
                                         "ValorFrete, " +
                                         "DataPedido, " +
                                         "P.Status, " +
                                         "Observacao, " +
                                         "SPD.Descricao " +
                                     "FROM Pedidos P " +
                                         "JOIN StatusDePedidos SPD ON(P.Status = SPD.Status) " +
                                     "WHERE P.Status = @Status " +
                                     "ORDER BY PedidoId";
                else
                    cmdTextoPedido = "SELECT " +
                                         "PedidoId, " +
                                         "UsuarioId, " +
                                         "EnderecoId, " +
                                         "ValorFrete, " +
                                         "DataPedido, " +
                                         "P.Status, " +
                                         "Observacao, " +
                                         "SPD.Descricao " +
                                     "FROM Pedidos P " +
                                     "JOIN StatusDePedidos SPD ON(P.Status = SPD.Status) " +
                                     "ORDER BY PedidoId";

                SqlCommand comandoPedido = new SqlCommand(cmdTextoPedido, conexao);

                if (pedido.UsuarioId > 0)
                    comandoPedido.Parameters.AddWithValue("@UsuarioId", pedido.UsuarioId);
                else if (pedido.Id > 0)
                    comandoPedido.Parameters.AddWithValue("@PedidoId", pedido.Id);
                if (pedido.Status != '\0')
                    comandoPedido.Parameters.AddWithValue("@Status", pedido.Status);

                SqlDataReader drPedido = comandoPedido.ExecuteReader();

                pedidos = DataReaderPedidoParaList(drPedido);

                //if (pedido.Status != 'P' && pedido.Status != 'R')
                //{
                foreach (var item in pedidos)
                {
                    cmdTextoPedido = "SELECT ItemId, Qtde, PrecoUnitario " +
                                     "FROM Pedidos P " +
                                     "JOIN PedidosItens PI ON(P.PedidoId = PI.PedidoId) " +
                                     "WHERE P.PedidoId = @PedidoId AND P.Status = @Status";

                    comandoPedido = new SqlCommand(cmdTextoPedido, conexao);

                    comandoPedido.Parameters.AddWithValue("@PedidoId", item.Id);
                    comandoPedido.Parameters.AddWithValue("@Status", item.Status);
                    drPedido = comandoPedido.ExecuteReader();

                    if (drPedido.HasRows)
                    {
                        while (drPedido.Read())
                        {
                            ItemPedido temp = new ItemPedido
                            {
                                Id = drPedido.GetInt32(0),
                                Qtde = drPedido.GetInt32(1),
                                Produto = new Domain.Produto.Livro { PrecoVenda = Convert.ToDouble(drPedido.GetDecimal(2)) }
                            };
                            item.ItensPedido.Add(temp);
                        }
                    }
                    drPedido.Close();
                }
                foreach (var item in pedidos)
                {
                    cmdTextoPedido = "SELECT C.CupomId, Tipo, Valor " +
                                     "FROM Cupons C " +
                                     "JOIN PedidosCupons PC ON(C.CupomId = PC.CupomId) " +
                                     "JOIN Pedidos P ON(PC.PedidoId = P.PedidoId) " +
                                     "WHERE P.PedidoId = @PedidoId";

                    comandoPedido = new SqlCommand(cmdTextoPedido, conexao);

                    comandoPedido.Parameters.AddWithValue("@PedidoId", item.Id);
                    drPedido = comandoPedido.ExecuteReader();

                    if (drPedido.HasRows)
                    {
                        item.CuponsTroca = new List<Cupom>();
                        while (drPedido.Read())
                        {
                            Cupom temp = new Cupom
                            {
                                Id = drPedido.GetInt32(0),
                                Tipo = Convert.ToChar(drPedido.GetString(1)),
                                Valor = Convert.ToDouble(drPedido.GetDecimal(2))
                            };
                            if (temp.Tipo == 'P')
                                item.CupomPromocional = temp;
                            else
                                item.CuponsTroca.Add(temp);
                        }
                    }
                    drPedido.Close();
                }
                //}
                comandoPedido.Dispose();
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
                return new List<Pedido>();

            List<Pedido> pedidos = new List<Pedido>();
            while (dataReader.Read())
            {
                try
                {
                    Pedido pedido = new Pedido
                    {
                        Id = Convert.ToInt32(dataReader["PedidoId"]),
                        UsuarioId = Convert.ToInt32(dataReader["UsuarioId"]),
                        EnderecoId = Convert.ToInt32(dataReader["EnderecoId"]),
                        DataCadastro = Convert.ToDateTime(dataReader["DataPedido"]),
                        Status = Convert.ToChar(dataReader["Status"]),
                    };
                    //if (!dataReader.IsDBNull(6))
                    //    pedido.Observacao = dataReader["Observacao"].ToString();
                    if (!Convert.IsDBNull(dataReader["ValorFrete"]))
                        pedido.ValorFrete = Convert.ToDouble(dataReader["ValorFrete"]);
                    if (!Convert.IsDBNull(dataReader["Observacao"]))
                        pedido.Observacao = dataReader["Observacao"].ToString();
                    if (!Convert.IsDBNull(dataReader["Descricao"]))
                        pedido.DescricaoStatus = dataReader["Descricao"].ToString();

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
