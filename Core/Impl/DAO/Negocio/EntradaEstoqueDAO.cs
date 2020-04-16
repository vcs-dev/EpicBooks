using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Impl.DAO.Negocio
{
    public class EntradaEstoqueDAO : AbstractDAO
    {
        public EntradaEstoqueDAO() : base("EntradaEstoque", "EntradaEstoqueId")
        {
        }

        public override void Salvar(EntidadeDominio entidade)
        {
            EntradaEstoque entradaEstoque = (EntradaEstoque)entidade;
            string cmdTextoEntradaEstoque;
            string cmdTextoEstoque;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextoEntradaEstoque = "INSERT INTO EntradaEstoque(" +
                                             "ProdutoId, " +
                                             "Qtde, " +
                                             "ValorCusto, " +
                                             "FornecedorId, " +
                                             "DataEntrada, " +
                                             "Observacao" +
                                         ") " +
                                         "VALUES(" +
                                             "@ProdutoId," +
                                             "@Qtde, " +
                                             "@ValorCusto, " +
                                             "@FornecedorId, " +
                                             "@DataEntrada, " +
                                             "@Observacao" +
                                         ") SELECT CAST(scope_identity() AS int)";

                SqlCommand comandoEntradaEstoque = new SqlCommand(cmdTextoEntradaEstoque, conexao, transacao);

                comandoEntradaEstoque.Parameters.AddWithValue("@ProdutoId", entradaEstoque.ProdutoId);
                comandoEntradaEstoque.Parameters.AddWithValue("@Qtde", entradaEstoque.Qtde);
                comandoEntradaEstoque.Parameters.AddWithValue("@ValorCusto", entradaEstoque.ValorCusto);
                comandoEntradaEstoque.Parameters.AddWithValue("@FornecedorId", entradaEstoque.FornecedorId);
                comandoEntradaEstoque.Parameters.AddWithValue("@DataEntrada", entradaEstoque.DataCadastro);
                if(entradaEstoque.Observacao != null)
                    comandoEntradaEstoque.Parameters.AddWithValue("@Observacao", entradaEstoque.DataCadastro);
                else
                    comandoEntradaEstoque.Parameters.AddWithValue("@Observacao", DBNull.Value);
                entradaEstoque.Id = Convert.ToInt32(comandoEntradaEstoque.ExecuteScalar());
                comandoEntradaEstoque.Dispose();

                cmdTextoEstoque = "UPDATE Estoque " +
                                  "SET Qtde = Qtde + @Qtde, " +
                                      "ValorCusto = ROUND((SELECT SUM(ValorCusto) FROM EntradaEstoque WHERE ProdutoId = @ProdutoId) / " +
                                      "(SELECT Count(ProdutoId) FROM EntradaEstoque WHERE ProdutoId = @ProdutoId), 2) " +
                                  "WHERE ProdutoId = @ProdutoId";
                SqlCommand comandoEstoque = new SqlCommand(cmdTextoEstoque, conexao, transacao);
                comandoEstoque.Parameters.AddWithValue("@ProdutoId", entradaEstoque.ProdutoId);
                comandoEstoque.Parameters.AddWithValue("@Qtde", entradaEstoque.Qtde);
                comandoEstoque.ExecuteNonQuery();
                comandoEstoque.Dispose();

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

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            EntradaEstoque entradaEstoque = (EntradaEstoque)entidade;
            List<EntradaEstoque> entradasEstoque = new List<EntradaEstoque>();
            string cmdTextoEntradaEstoque;

            try
            {
                Conectar();

                if (entradaEstoque.ProdutoId > 0)
                    cmdTextoEntradaEstoque = "SELECT * FROM EntradaEstoque WHERE ProdutoId = @ProdutoId";
                else
                    cmdTextoEntradaEstoque = "SELECT * FROM EntradaEstoque";

                SqlCommand comandoestoque = new SqlCommand(cmdTextoEntradaEstoque, conexao);

                if (entradaEstoque.ProdutoId > 0)
                    comandoestoque.Parameters.AddWithValue("@ProdutoId", entradaEstoque.ProdutoId);

                SqlDataReader drEntradaEstoque = comandoestoque.ExecuteReader();
                comandoestoque.Dispose();

                entradasEstoque = DataReaderEntradaEstoqueParaList(drEntradaEstoque);
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
            return entradasEstoque.ToList<EntidadeDominio>();
        }

        public List<EntradaEstoque> DataReaderEntradaEstoqueParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
            {
                throw new Exception("Sem Registros");
            }

            List<EntradaEstoque> estoqueProds = new List<EntradaEstoque>();
            while (dataReader.Read())
            {
                try
                {
                    EntradaEstoque entradaEstoque = new EntradaEstoque
                    {
                        Id = Convert.ToInt32(dataReader["EntradaEstoqueId"]),
                        ProdutoId = Convert.ToInt32(dataReader["ProdutoId"]),
                        Qtde = Convert.ToInt32(dataReader["Qtde"]),
                        ValorCusto = Convert.ToDouble(dataReader["ValorCusto"]),
                        FornecedorId = Convert.ToInt32(dataReader["FornecedorId"]),
                        DataCadastro = Convert.ToDateTime(dataReader["DataEntrada"]),
                    };
                    if (!Convert.IsDBNull(dataReader["Observacao"]))
                        entradaEstoque.Observacao = dataReader["Observacao"].ToString();
                    estoqueProds.Add(entradaEstoque);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return estoqueProds.ToList();
        }
    }
}
