using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Impl.DAO.Negocio
{
    public class VendaDAO : AbstractDAO
    {
        public VendaDAO() : base("", "")
        {
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            Venda venda = (Venda)entidade;
            List<Venda> vendas = new List<Venda>();
            string cmdTextoGrafico;

            try
            {
                Conectar();
                cmdTextoGrafico = "SELECT Nome, SUM(Qtde) AS Qtde FROM(" +
                                        "SELECT  G.Nome, " +
                                        "      PI.Qtde " +
                                        "FROM Produtos PR " +
                                        "    JOIN ProdutosGeneros PG ON(PR.ProdutoId = PG.ProdutoId) " +
                                        "    JOIN Generos G ON(PG.GeneroId = G.GeneroId) " +
                                        "    JOIN PedidosItens PI ON(PR.ProdutoId = PI.ItemId) " +
                                        "    JOIN Pedidos P ON(PI.PedidoId = P.PedidoId) " +
                                        "    JOIN Vendas V ON(PI.PedidoId = V.PedidoId) " +
                                        "WHERE V.DataVenda BETWEEN @DataInicial AND @DataFinal " +
                                        "GROUP BY G.Nome, PI.Qtde " +
                                        ") AS Tb " +
                                    "GROUP BY Nome ";
                SqlCommand comandoVenda = new SqlCommand(cmdTextoGrafico, conexao);

                comandoVenda.Parameters.AddWithValue("@DataInicial", Convert.ToDateTime(venda.DataInicial));
                comandoVenda.Parameters.AddWithValue("@DataFinal", Convert.ToDateTime(venda.DataFinal));

                SqlDataReader drGrafico = comandoVenda.ExecuteReader();
                comandoVenda.Parameters.Clear();

                vendas = DataReaderGraficoParaList(drGrafico);

                comandoVenda.Dispose();
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
            return vendas.ToList<EntidadeDominio>();
        }

        public List<Venda> DataReaderGraficoParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
                return new List<Venda>();

            List<Venda> vendas = new List<Venda>();
            while (dataReader.Read())
            {
                try
                {
                    Venda venda = new Venda
                    {
                        NomeProduto = dataReader["Nome"].ToString(),
                        Qtde = Convert.ToInt32(dataReader["Qtde"])
                    };

                    vendas.Add(venda);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return vendas.ToList();
        }
    }
}
