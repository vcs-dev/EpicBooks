using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Impl.DAO.Negocio
{
    public class EstoqueDAO : AbstractDAO
    {
        public EstoqueDAO() : base("Estoque", "EstoqueId")
        {
        }
        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            Estoque estoque = (Estoque)entidade;
            List<Estoque> estoqueProds = new List<Estoque>();
            string cmdTextoEstoque;

            try
            {
                Conectar();

                if (estoque.ProdutoId > 0)
                    cmdTextoEstoque = "SELECT " +
                                          "E.EstoqueId, " +
                                          "E.ProdutoId, " +
                                          "E.Qtde, " +
                                          "E.ValorCusto, " +
                                          "E.Observacao, " +
                                          "PR.Nome, " +
                                          "PR.CaminhoImagem " +
                                      "FROM Estoque E " +
                                      "JOIN Produtos PR ON(E.ProdutoId = PR.ProdutoId) " + 
                                      "WHERE E.ProdutoId = @ProdutoId";
                else if (estoque.NomeProduto != null)
                    cmdTextoEstoque = "SELECT " +
                                          "E.EstoqueId, " +
                                          "E.ProdutoId, " +
                                          "E.Qtde, " +
                                          "E.ValorCusto, " +
                                          "E.Observacao, " +
                                          "PR.Nome, " +
                                          "PR.CaminhoImagem " +
                                      "FROM Estoque E " +
                                      "JOIN Produtos PR ON(E.ProdutoId = PR.ProdutoId) " +
                                      "WHERE PR.Nome LIKE @Nome";
                else
                    cmdTextoEstoque = "SELECT " +
                                          "E.EstoqueId, " +
                                          "E.ProdutoId, " +
                                          "E.Qtde, " +
                                          "E.ValorCusto, " +
                                          "E.Observacao, " +
                                          "PR.Nome, " +
                                          "PR.CaminhoImagem " +
                                      "FROM Estoque E " +
                                      "JOIN Produtos PR ON(E.ProdutoId = PR.ProdutoId)";

                SqlCommand comandoestoque = new SqlCommand(cmdTextoEstoque, conexao);

                if (estoque.ProdutoId > 0)
                    comandoestoque.Parameters.AddWithValue("@ProdutoId", estoque.ProdutoId);
                if (estoque.NomeProduto != null)
                    comandoestoque.Parameters.AddWithValue("@Nome", "%" + estoque.NomeProduto + "%");

                SqlDataReader drEstoque = comandoestoque.ExecuteReader();
                comandoestoque.Dispose();

                estoqueProds = DataReaderEstoqueParaList(drEstoque);
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
            return estoqueProds.ToList<EntidadeDominio>();
        }
        public List<Estoque> DataReaderEstoqueParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
            {
                throw new Exception("Sem Registros");
            }

            List<Estoque> estoqueProds = new List<Estoque>();
            while (dataReader.Read())
            {
                try
                {
                    Estoque estoque = new Estoque
                    {
                        Id = Convert.ToInt32(dataReader["EstoqueId"]),
                        ProdutoId = Convert.ToInt32(dataReader["ProdutoId"]),
                        Qtde = Convert.ToInt32(dataReader["Qtde"]),
                        ValorCusto = Convert.ToDouble(dataReader["ValorCusto"]),
                        CaminhoImagem = dataReader["CaminhoImagem"].ToString(),
                        NomeProduto = dataReader["Nome"].ToString(),
                    };
                    if (!Convert.IsDBNull(dataReader["Observacao"]))
                        estoque.Observacao = dataReader["CaminhoImagem"].ToString();
                    estoqueProds.Add(estoque);
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
