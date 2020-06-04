using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Core.Impl.DAO.Negocio
{
    public class FaturamentoDAO : AbstractDAO
    {
        public FaturamentoDAO():base("", "")
        {
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            Faturamento faturamento = (Faturamento)entidade;
            List<Faturamento> faturamentos = new List<Faturamento>();
            string cmdTextoFaturamento;

            try
            {
                Conectar();
                cmdTextoFaturamento = "SELECT PedidoId, SUM(ValorItem) AS ValorItem, ValorFrete " +
                                    "INTO #tmpValoresItensEFrete " +
                                    "FROM " +
                                    "(" +
                                        "SELECT DISTINCT P.PedidoId, " +
                                        "(PI.PrecoUnitario* PI.Qtde) AS ValorItem, " +
                                        "P.ValorFrete " +
                                        "FROM Pedidos P " +
                                            "JOIN PedidosItens PI ON(P.PedidoId = PI.PedidoId) " +
                                    ") AS TB " +
                                    "GROUP BY PedidoId, ValorFrete " +

                                    "SELECT PedidoId, SUM(ValorCupom) AS ValorCupom " +
                                    "INTO #tmpValoresCupons " +
                                    "FROM " +
                                    "( " +
                                    "    SELECT DISTINCT P.PedidoId, " +
                                    "            C.Valor AS ValorCupom, " +
                                    "            P.ValorFrete " +
                                    "            FROM Pedidos P " +
                                    "                JOIN PedidosItens PI ON(P.PedidoId = PI.PedidoId) " +
                                    "                JOIN PedidosCupons PC ON(PC.PedidoId = PI.PedidoId) " +
                                    "                JOIN Cupons C ON(C.CupomId = PC.CupomId) " +
                                    ") AS TB " +
                                    "GROUP BY PedidoId, ValorFrete " +
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
                                            "      FORMAT(V.DataVenda, 'MM/yyyy') AS Data, " +
                                            "    (TMP1.ValorItem + TMP1.ValorFrete - ISNULL(TMP2.ValorCupom, 0)) AS ValorTotalPedido " +
                                            "FROM Pedidos P " +
                                                "JOIN #tmpValoresItensEFrete TMP1 ON(P.PedidoId = TMP1.PedidoId) " +
                                                "LEFT JOIN #tmpValoresCupons TMP2 ON(TMP1.PedidoId = TMP2.PedidoId) " +
                                                "JOIN StatusDePedidos SDP ON(P.Status = SDP.Status) " +
                                                "JOIN Vendas V ON(P.PedidoId = V.PedidoId) " +
                                            "WHERE P.Status <> 'P' AND P.Status <> 'R' AND P.Status <> 'C' AND " +
                                            "V.DataVenda BETWEEN @DataInicial AND @DataFinal " +
                                        ")AS Tb " +
                                    ")AS Tb2 " +
                                    "GROUP BY Data " +
                                    "DROP TABLE #tmpValoresItensEFrete " +
                                    "DROP TABLE #tmpValoresCupons ";

                SqlCommand comandoFaturamento = new SqlCommand(cmdTextoFaturamento, conexao);

                comandoFaturamento.Parameters.AddWithValue("@DataInicial", Convert.ToDateTime(faturamento.DataInicial));
                comandoFaturamento.Parameters.AddWithValue("@DataFinal", Convert.ToDateTime(faturamento.DataFinal));

                SqlDataReader drFaturamento = comandoFaturamento.ExecuteReader();
                comandoFaturamento.Parameters.Clear();

                faturamentos = DataReaderFaturamentoParaList(drFaturamento);

                comandoFaturamento.Dispose();
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
            return faturamentos.ToList<EntidadeDominio>();
        }

        public List<Faturamento> DataReaderFaturamentoParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
                return new List<Faturamento>();

            List<Faturamento> faturamentos = new List<Faturamento>();
            while (dataReader.Read())
            {
                try
                {
                    Faturamento faturamento = new Faturamento
                    {
                        Valor = Convert.ToDouble(dataReader["Valor"]),
                        Data = dataReader["Data"].ToString()
                    };
                    faturamentos.Add(faturamento);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return faturamentos;
        }

    }
}
