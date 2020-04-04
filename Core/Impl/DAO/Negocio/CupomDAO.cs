using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Impl.DAO.Negocio
{
    public class CupomDAO : AbstractDAO
    {
        public CupomDAO() : base("Cupons", "CupomId")
        {
        }
        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            Cupom cupom = (Cupom)entidade;
            List<Cupom> cupons;
            string cmdTextoCupom = "";

            try
            {
                Conectar();

                if (!string.IsNullOrEmpty(cupom.CodigoCupom) && !string.IsNullOrWhiteSpace(cupom.CodigoCupom))
                    cmdTextoCupom = "SELECT * FROM Cupons WHERE CodigoCupom = @CodigoCupom";
                else if (cupom.UsuarioId != null && cupom.UsuarioId != 0)
                    cmdTextoCupom = "SELECT * FROM Cupons WHERE UsuarioId = @UsuarioId";

                SqlCommand comandoCupom = new SqlCommand(cmdTextoCupom, conexao);

                if (!string.IsNullOrEmpty(cupom.CodigoCupom) && !string.IsNullOrWhiteSpace(cupom.CodigoCupom))
                    comandoCupom.Parameters.AddWithValue("@CodigoCupom", cupom.CodigoCupom);
                if (cupom.UsuarioId != null && cupom.UsuarioId != 0)
                    comandoCupom.Parameters.AddWithValue("@UsuarioId", cupom.UsuarioId);

                SqlDataReader drCupom = comandoCupom.ExecuteReader();
                comandoCupom.Dispose();

                cupons = DataReadercupomParaList(drCupom);
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
            return cupons.ToList<EntidadeDominio>();
        }
        public List<Cupom> DataReadercupomParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
            {
                throw new Exception("Sem Registros");
            }

            List<Cupom> cupons = new List<Cupom>();
            while (dataReader.Read())
            {
                try
                {
                    Cupom cupom = new Cupom
                    {
                        Id = Convert.ToInt32(dataReader["CupomId"]),
                        CodigoCupom = dataReader["CodigoCupom"].ToString(),
                        Status = Convert.ToByte(dataReader["Status"]),
                        TipoCupom = Convert.ToChar(dataReader["TipoCupom"]),
                        ValorCupom = Convert.ToDouble(dataReader["ValorCupom"]),
                    };
                    if (!dataReader.IsDBNull(5))
                        cupom.UsuarioId = Convert.ToInt32(dataReader["UsuarioId"]);

                    cupons.Add(cupom);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return cupons.ToList();
        }
    }
}
