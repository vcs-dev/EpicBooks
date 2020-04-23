using Domain;
using Domain.DadosCliente;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Impl.DAO.DadosCliente
{
    public class EnderecoDAO : AbstractDAO
    {
        public EnderecoDAO():base("Enderecos", "EnderecoDAO")
        {
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            Endereco endereco = (Endereco)entidade;
            List<Endereco> enderecos;
            string cmdTextoEndereco = "";

            try
            {
                Conectar();

                if (endereco.Id > 0)
                    cmdTextoEndereco = "SELECT * FROM Enderecos WHERE EnderecoId = @EnderecoId";
                else if (endereco.UsuarioId > 0)
                    cmdTextoEndereco = "SELECT * " +
                                       "FROM Enderecos E " +
                                           "INNER JOIN UsuariosEnderecos UE ON(E.EnderecoId = UE.EnderecoId) " +
                                       "WHERE UE.UsuarioId = @UsuarioId";

                SqlCommand comandoEndereco = new SqlCommand(cmdTextoEndereco, conexao);

                if (endereco.Id > 0)
                    comandoEndereco.Parameters.AddWithValue("@EnderecoId", endereco.Id);
                if (endereco.UsuarioId > 0)
                    comandoEndereco.Parameters.AddWithValue("@UsuarioId", endereco.UsuarioId);

                SqlDataReader drEndereco = comandoEndereco.ExecuteReader();
                comandoEndereco.Dispose();

                enderecos = DataReaderenderecoParaList(drEndereco);
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
            return enderecos.ToList<EntidadeDominio>();
        }
        public List<Endereco> DataReaderenderecoParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
                return new List<Endereco>();

            List<Endereco> enderecos = new List<Endereco>();
            while (dataReader.Read())
            {
                try
                {
                    Endereco endereco = new Endereco
                    {
                        Id = Convert.ToInt32(dataReader["EnderecoId"]),
                        TipoEndereco = Convert.ToInt32(dataReader["TipoEndereco"]),
                        TipoResidencia = Convert.ToInt32(dataReader["TipoResidencia"]),
                        TipoLogradouro = Convert.ToInt32(dataReader["TipoLogradouro"]),
                        Logradouro = dataReader["Logradouro"].ToString(),
                        Numero = Convert.ToInt32(dataReader["Numero"]),
                        Cep = dataReader["Cep"].ToString(),
                        Bairro = dataReader["Bairro"].ToString(),
                        Cidade = Convert.ToInt32(dataReader["Cidade"]),
                        Estado = Convert.ToInt32(dataReader["Estado"]),
                        Pais = Convert.ToInt32(dataReader["Pais"]),
                    };
                    if(!Convert.IsDBNull(dataReader["Observacao"]))
                        endereco.Observacao = dataReader["Observacao"].ToString();

                    enderecos.Add(endereco);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return enderecos;
        }
    }
}
