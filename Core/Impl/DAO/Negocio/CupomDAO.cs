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

                if (!string.IsNullOrEmpty(cupom.Codigo) && !string.IsNullOrWhiteSpace(cupom.Codigo) &&
                    cupom.DataExpiracao != DateTime.MinValue && cupom.Usado != null)
                    cmdTextoCupom = "SELECT * FROM Cupons WHERE Codigo = @Codigo AND  DataExpiracao >= @DataExpiracao AND Usado = @Usado";
                else if (!string.IsNullOrEmpty(cupom.Codigo) && !string.IsNullOrWhiteSpace(cupom.Codigo) &&
                    cupom.DataExpiracao != DateTime.MinValue && cupom.Usado == null)
                    cmdTextoCupom = "SELECT * FROM Cupons WHERE Codigo = @Codigo AND  DataExpiracao >= @DataExpiracao";
                else if (cupom.Tipo != '\0' &&  cupom.Tipo != ' ' &&
                    cupom.DataExpiracao != DateTime.MinValue && cupom.DataExpiracao != null && cupom.Usado != null)
                    cmdTextoCupom = "SELECT * FROM Cupons WHERE Tipo = @Tipo AND  DataExpiracao >= @DataExpiracao AND Usado = @Usado";
                else if (cupom.UsuarioId != null && cupom.UsuarioId != 0 &&
                    cupom.DataExpiracao != DateTime.MinValue && cupom.DataExpiracao != null && cupom.Usado != null)
                    cmdTextoCupom = "SELECT * FROM Cupons WHERE UsuarioId = @UsuarioId AND  DataExpiracao >= @DataExpiracao AND Usado = @Usado";
                else if(cupom.UsuarioId != null && cupom.UsuarioId != 0)
                    cmdTextoCupom = "SELECT * FROM Cupons WHERE UsuarioId = @UsuarioId";
                else
                    cmdTextoCupom = "SELECT * FROM Cupons";

                SqlCommand comandoCupom = new SqlCommand(cmdTextoCupom, conexao);

                if (!string.IsNullOrEmpty(cupom.Codigo) && !string.IsNullOrWhiteSpace(cupom.Codigo))
                    comandoCupom.Parameters.AddWithValue("@Codigo", cupom.Codigo);
                if (cupom.UsuarioId != null && cupom.UsuarioId != 0)
                    comandoCupom.Parameters.AddWithValue("@UsuarioId", cupom.UsuarioId);
                if (cupom.DataExpiracao != DateTime.MinValue && cupom.DataExpiracao != null)
                    comandoCupom.Parameters.AddWithValue("@DataExpiracao", cupom.DataExpiracao);
                if (cupom.Usado != null)
                    comandoCupom.Parameters.AddWithValue("@Usado", cupom.Usado);
                if (cupom.Tipo != '\0' && cupom.Tipo != ' ')
                    comandoCupom.Parameters.AddWithValue("@Tipo", cupom.Tipo);

                SqlDataReader drCupom = comandoCupom.ExecuteReader();
                comandoCupom.Dispose();

                cupons = DataReaderCupomParaList(drCupom);
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
        public List<Cupom> DataReaderCupomParaList(SqlDataReader dataReader)
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
                        Codigo = dataReader["Codigo"].ToString(),
                        Tipo = Convert.ToChar(dataReader["Tipo"]),
                        Valor = Convert.ToDouble(dataReader["Valor"]),
                    };
                    if (!dataReader.IsDBNull(4))
                        cupom.DataExpiracao = Convert.ToDateTime(dataReader["DataExpiracao"]);
                    if (!dataReader.IsDBNull(5))
                        cupom.Usado = Convert.ToByte(dataReader["Usado"]);
                    if (!dataReader.IsDBNull(6))
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
