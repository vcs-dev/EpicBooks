using Domain;
using Domain.Negocio;
using Domain.Produto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Core.Impl.DAO.Negocio
{
    public class ItemPedidoDAO : AbstractDAO
    {
        public ItemPedidoDAO():base("PedidosItensItens", "PedidoItemId")
        {
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            Pedido pedido = (Pedido)entidade;
            List<ItemPedido> itensPed = new List<ItemPedido>();
            string cmdTextoItemPedido;

            try
            {
                Conectar();

                    cmdTextoItemPedido = "SELECT * FROM PedidosItens WHERE PedidoId = @PedidoId";

                SqlCommand comandoItemPedido = new SqlCommand(cmdTextoItemPedido, conexao);

                comandoItemPedido.Parameters.AddWithValue("@PedidoId", pedido.Id);

                SqlDataReader drPedidosItens = comandoItemPedido.ExecuteReader();
                comandoItemPedido.Parameters.Clear();

                itensPed = DataReaderPedidoParaList(drPedidosItens);

                comandoItemPedido.Dispose();
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
            return itensPed.ToList<EntidadeDominio>();
        }
        public List<ItemPedido> DataReaderPedidoParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
            {
                throw new Exception("Sem Registros");
            }

            List<ItemPedido> itensPed = new List<ItemPedido>();
            while (dataReader.Read())
            {
                try
                {
                    ItemPedido itemPed = new ItemPedido
                    {
                        Id = Convert.ToInt32(dataReader["PedidoItemId"]),
                        PedidoId = Convert.ToInt32(dataReader["PedidoId"]),
                        Produto = new Livro 
                        {
                            Id = Convert.ToInt32(dataReader["ItemId"]), 
                            PrecoVenda = Convert.ToDouble(dataReader["PrecoUnitario"]) 
                        },
                        Qtde = Convert.ToInt32(dataReader["Qtde"]),
                    };
                    itensPed.Add(itemPed);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return itensPed.ToList();
        }
    }
}
