using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Core.Impl.DAO.Negocio
{
    public class LucroDAO : AbstractDAO
    {
        public LucroDAO():base("", "")
        {
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            Lucro lucro = (Lucro)entidade;
            List<Lucro> lucros = new List<Lucro>();
            string cmdTextoLucro;

            try
            {
                Conectar();
                cmdTextoLucro = "SELECT PedidoId, SUM(ValorItem) AS ValorItem " +
                                "INTO #tmpValoresItens " +
                                "FROM " +
                                "(" +
                                    "SELECT DISTINCT P.PedidoId, " +
                                    "((PI.PrecoUnitario - E.ValorCusto) * PI.Qtde) AS ValorItem " +
                                    "FROM Pedidos P " +
                                        "JOIN PedidosItens PI ON(P.PedidoId = PI.PedidoId) " +
                                        "JOIN Estoque E ON(PI.ItemId = E.ProdutoId)" +
                                ") AS TB " +
                                "GROUP BY PedidoId " +

                                "SELECT PedidoId, SUM(ValorCupom) AS ValorCupom " +
                                "INTO #tmpValoresCupons " +
                                "FROM " +
                                "( " +
                                "    SELECT DISTINCT P.PedidoId, " +
                                "            C.Valor AS ValorCupom " +
                                "            FROM Pedidos P " +
                                "                JOIN PedidosItens PI ON(P.PedidoId = PI.PedidoId) " +
                                "                JOIN PedidosCupons PC ON(PC.PedidoId = PI.PedidoId) " +
                                "                JOIN Cupons C ON(C.CupomId = PC.CupomId) " +
                                ") AS TB " +
                                "GROUP BY PedidoId " +
                                "SELECT SUM(ValorTotalPedido) AS Valor, Data " +
                                "FROM " +
                                "(" +
                                    "SELECT " +
                                    "  PedidoId, " +
                                    "  Data, " +
                                    "  CASE " +
                                    "        WHEN ValorTotalPedido < 0 THEN 0 " +
                                    "        ELSE ValorTotalPedido " +
                                    "    END AS ValorTotalPedido " +
                                    "FROM " +
                                    "(" +
                                        "SELECT " +
                                        "    P.PedidoId, " +
                                        "    FORMAT(V.DataVenda, 'MM/yyyy') AS Data, " +
                                        "    CASE " +
                                        "        WHEN(TMP1.ValorItem - ISNULL(TMP2.ValorCupom, 0)) < 0 THEN 0 " +
                                        "        ELSE(TMP1.ValorItem - ISNULL(TMP2.ValorCupom, 0)) " +
                                        "    END AS ValorTotalPedido " +
                                        "FROM Pedidos P " +
                                            "JOIN #tmpValoresItens TMP1 ON(P.PedidoId = TMP1.PedidoId) " +
                                            "LEFT JOIN #tmpValoresCupons TMP2 ON(TMP1.PedidoId = TMP2.PedidoId) " +
                                            "JOIN StatusDePedidos SDP ON(P.Status = SDP.Status) " +
                                            "JOIN Vendas V ON(P.PedidoId = V.PedidoId) " +
                                        "WHERE P.Status <> 'P' AND P.Status <> 'R' AND P.Status <> 'C' AND " +
                                        "V.DataVenda BETWEEN @DataInicial AND @DataFinal " +
                                    ")AS Tb " +
                                ")AS Tb2 " +
                                "GROUP BY Data " +
                                "DROP TABLE #tmpValoresItens " +
                                "DROP TABLE #tmpValoresCupons ";

                SqlCommand comandoLucro = new SqlCommand(cmdTextoLucro, conexao);

                comandoLucro.Parameters.AddWithValue("@DataInicial", Convert.ToDateTime(lucro.DataInicial));
                comandoLucro.Parameters.AddWithValue("@DataFinal", Convert.ToDateTime(lucro.DataFinal));

                SqlDataReader drLucro = comandoLucro.ExecuteReader();
                comandoLucro.Parameters.Clear();

                lucros = DataReaderLucroParaList(drLucro);

                comandoLucro.Dispose();
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
            return lucros.ToList<EntidadeDominio>();
        }

        public List<Lucro> DataReaderLucroParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
                return new List<Lucro>();

            List<Lucro> lucros = new List<Lucro>();
            while (dataReader.Read())
            {
                try
                {
                    Lucro lucro = new Lucro
                    {
                        Valor = Convert.ToDouble(dataReader["Valor"]),
                        Data = dataReader["Data"].ToString()
                    };
                    lucros.Add(lucro);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return lucros;
        }

    }
}
