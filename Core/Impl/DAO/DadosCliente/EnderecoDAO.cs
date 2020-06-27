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
        public EnderecoDAO():base("Enderecos", "EnderecoId")
        {
        }

        public override void Salvar(EntidadeDominio entidade)
        {
            Endereco endereco = (Endereco)entidade;
            string cmdTextoEndereco;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextoEndereco = "INSERT INTO Enderecos(TipoEndereco," +
                                                         "TipoResidencia," +
                                                         "TipoLogradouro," +
                                                         "Logradouro," +
                                                         "Numero," +
                                                         "Cep," +
                                                         "Bairro," +
                                                         "Cidade," +
                                                         "Estado," +
                                                         "Pais," +
                                                         "Observacao, " +
                                                         "Ativo" +
                                  ") " +
                                  "VALUES(@TipoEndereco," +
                                         "@TipoResidencia," +
                                         "@TipoLogradouro," +
                                         "@Logradouro,"  +
                                         "@Numero," +
                                         "@Cep," +
                                         "@Bairro," +
                                         "@Cidade," +
                                         "@Estado," +
                                         "@Pais," +
                                         "@Observacao, " +
                                         "@Ativo" +
                                  ") SELECT CAST(scope_identity() AS int)";

                SqlCommand comandoEndereco = new SqlCommand(cmdTextoEndereco, conexao, transacao);

                comandoEndereco.Parameters.AddWithValue("@TipoEndereco", endereco.TipoEndereco);
                comandoEndereco.Parameters.AddWithValue("@TipoResidencia", endereco.TipoResidencia);
                comandoEndereco.Parameters.AddWithValue("@TipoLogradouro", endereco.TipoLogradouro);
                comandoEndereco.Parameters.AddWithValue("@Logradouro", endereco.Logradouro);
                comandoEndereco.Parameters.AddWithValue("@Numero", endereco.Numero);
                comandoEndereco.Parameters.AddWithValue("@Cep", endereco.Cep);
                comandoEndereco.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                comandoEndereco.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                comandoEndereco.Parameters.AddWithValue("@Estado", endereco.Estado);
                comandoEndereco.Parameters.AddWithValue("@Pais", endereco.Pais);
                comandoEndereco.Parameters.AddWithValue("@Ativo", endereco.Ativo);
                if (string.IsNullOrEmpty(endereco.Observacao))
                    comandoEndereco.Parameters.AddWithValue("@Observacao", DBNull.Value);
                else
                    comandoEndereco.Parameters.AddWithValue("@Observacao", endereco.Observacao);
                endereco.Id = Convert.ToByte(comandoEndereco.ExecuteScalar());

                cmdTextoEndereco = "INSERT INTO UsuariosEnderecos(UsuarioId," +
                                                                 "EnderecoId" +
                                               ") " +
                                                          "VALUES(@UsuarioId," +
                                                                 "@EnderecoId" +
                                               ")";

                comandoEndereco = new SqlCommand(cmdTextoEndereco, conexao, transacao);

                comandoEndereco.Parameters.AddWithValue("@UsuarioId", endereco.UsuarioId);
                comandoEndereco.Parameters.AddWithValue("@EnderecoId", endereco.Id);
                comandoEndereco.ExecuteNonQuery();
                comandoEndereco.Dispose();

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

        public override void Alterar(EntidadeDominio entidade)
        {
            Endereco endereco = (Endereco)entidade;
            string cmdTextoEndereco;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextoEndereco = "UPDATE Enderecos SET TipoEndereco = @TipoEndereco," +
                                                         "TipoResidencia = @TipoResidencia," +
                                                         "TipoLogradouro = @TipoLogradouro," +
                                                         "Logradouro = @Logradouro," +
                                                         "Numero = @Numero," +
                                                         "Cep = @Cep," +
                                                         "Bairro = @Bairro," +
                                                         "Cidade = @Cidade," +
                                                         "Estado = @Estado," +
                                                         "Pais = @Pais," +
                                                         "Observacao = @Observacao, " +
                                                         "Ativo = @Ativo " + 
                                   "WHERE EnderecoId = @EnderecoId";

                SqlCommand comandoEndereco = new SqlCommand(cmdTextoEndereco, conexao, transacao);

                comandoEndereco.Parameters.AddWithValue("@TipoEndereco", endereco.TipoEndereco);
                comandoEndereco.Parameters.AddWithValue("@TipoResidencia", endereco.TipoResidencia);
                comandoEndereco.Parameters.AddWithValue("@TipoLogradouro", endereco.TipoLogradouro);
                comandoEndereco.Parameters.AddWithValue("@Logradouro", endereco.Logradouro);
                comandoEndereco.Parameters.AddWithValue("@Numero", endereco.Numero);
                comandoEndereco.Parameters.AddWithValue("@Cep", endereco.Cep);
                comandoEndereco.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                comandoEndereco.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                comandoEndereco.Parameters.AddWithValue("@Estado", endereco.Estado);
                comandoEndereco.Parameters.AddWithValue("@Pais", endereco.Pais);
                if (string.IsNullOrEmpty(endereco.Observacao))
                    comandoEndereco.Parameters.AddWithValue("@Observacao", DBNull.Value);
                else
                    comandoEndereco.Parameters.AddWithValue("@Observacao", endereco.Observacao);
                comandoEndereco.Parameters.AddWithValue("@Ativo", endereco.Ativo);
                comandoEndereco.Parameters.AddWithValue("@EnderecoId", endereco.Id);
                comandoEndereco.ExecuteNonQuery();
                comandoEndereco.Dispose();

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
            Endereco endereco = (Endereco)entidade;
            List<Endereco> enderecos;
            string cmdTextoEndereco = "";

            try
            {
                Conectar();

                if (endereco.Id > 0 && endereco.TipoEndereco == 0)
                    cmdTextoEndereco = "SELECT * FROM Enderecos WHERE EnderecoId = @EnderecoId AND Ativo = 1";
                else if (endereco.Id > 0 && endereco.TipoEndereco > 0)
                    cmdTextoEndereco = "SELECT * FROM Enderecos WHERE EnderecoId = @EnderecoId AND TipoEndereco = @TipoEndereco  AND Ativo = 1";
                else if (endereco.UsuarioId > 0 && endereco.TipoEndereco == 0)
                    cmdTextoEndereco = "SELECT * " +
                                       "FROM Enderecos E " +
                                           "INNER JOIN UsuariosEnderecos UE ON(E.EnderecoId = UE.EnderecoId) " +
                                       "WHERE UE.UsuarioId = @UsuarioId  AND Ativo = 1";
                else if (endereco.UsuarioId > 0 && endereco.TipoEndereco > 0)
                    cmdTextoEndereco = "SELECT * " +
                                       "FROM Enderecos E " +
                                           "INNER JOIN UsuariosEnderecos UE ON(E.EnderecoId = UE.EnderecoId) " +
                                       "WHERE UE.UsuarioId = @UsuarioId AND TipoEndereco = @TipoEndereco  AND Ativo = 1";

                SqlCommand comandoEndereco = new SqlCommand(cmdTextoEndereco, conexao);

                if (endereco.Id > 0)
                    comandoEndereco.Parameters.AddWithValue("@EnderecoId", endereco.Id);
                if (endereco.UsuarioId > 0)
                    comandoEndereco.Parameters.AddWithValue("@UsuarioId", endereco.UsuarioId);
                if(endereco.TipoEndereco > 0)
                    comandoEndereco.Parameters.AddWithValue("@TipoEndereco", endereco.TipoEndereco);

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
