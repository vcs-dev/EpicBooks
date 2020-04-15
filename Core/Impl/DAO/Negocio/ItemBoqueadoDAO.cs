using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Impl.DAO.Negocio
{
    public class ItemBoqueadoDAO : AbstractDAO
    {
        public ItemBoqueadoDAO(): base("ItensBloqueados", "ItemId")
        {       
        }
        public override void Salvar(EntidadeDominio entidade)
        {
            ItemBloqueado itemBloq = (ItemBloqueado)entidade;
            string cmdTextoItemPed;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextoItemPed = "INSERT INTO ItensBloqueados" +
                                    "(ItemId" +
                                  ") " +
                                  "VALUES" +
                                      "(@ItemId)" + 
                                  "SELECT CAST(scope_identity() AS int)";

                SqlCommand comandoItemPed = new SqlCommand(cmdTextoItemPed, conexao, transacao);

                comandoItemPed.Parameters.AddWithValue("@ItemId", itemBloq.Id);
                itemBloq.Id = Convert.ToInt32(comandoItemPed.ExecuteScalar());
                comandoItemPed.Dispose();

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
            ItemBloqueado itemBloq = (ItemBloqueado)entidade;
            List<ItemBloqueado> itensPed;
            string cmdTextoItem;

            try
            {
                Conectar();

                cmdTextoItem = "SELECT * FROM ItensBloqueados WHERE ItemId = @ItemId";


                SqlCommand comandoItem = new SqlCommand(cmdTextoItem, conexao);

                comandoItem.Parameters.AddWithValue("@ItemId", itemBloq.Id);

                SqlDataReader drItensPedido = comandoItem.ExecuteReader();
                comandoItem.Dispose();

                itensPed = DataReaderCartaoParaList(drItensPedido);
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

        public List<ItemBloqueado> DataReaderCartaoParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
                return new List<ItemBloqueado>();

            List<ItemBloqueado> itensPed = new List<ItemBloqueado>();
            while (dataReader.Read())
            {
                try
                {
                    ItemBloqueado itemPed = new ItemBloqueado
                    {
                        Id = Convert.ToInt32(dataReader["ItemId"]),
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
